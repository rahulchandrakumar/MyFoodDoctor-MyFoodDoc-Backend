using Microsoft.Azure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Course;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Courses;
using MyFoodDoc.Application.Enums;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApplicationContext _context;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly IAppStoreClient _appStoreClient;
        private readonly IGooglePlayStoreClient _googlePlayStoreClient;
        private readonly ILogger<CourseService> _logger;
        private readonly string _templateUrl;

        public CourseService(
            IConfiguration configuration,
            IApplicationContext context,
            IPdfService pdfService,
            IEmailService emailService,
            IAppStoreClient appStoreClient,
            IGooglePlayStoreClient googlePlayStoreClient,
            ILogger<CourseService> logger)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
            _appStoreClient = appStoreClient;
            _googlePlayStoreClient = googlePlayStoreClient;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var connectionString = configuration.GetConnectionString("BlobStorageConnection");

            var cloudStorageUrl = CloudStorageAccount.Parse(connectionString).BlobStorageUri.PrimaryUri.AbsoluteUri;

            _templateUrl = cloudStorageUrl + @"templates/teilnahmebescheinigung-praeventionskurs.pdf";
        }

        public async Task<ICollection<CourseDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<CourseDto>();

            var userAnswers = await _context.UserAnswers.Where(x => x.UserId == userId).ToListAsync(cancellationToken);

            var isNextCourseAvailable = true;

            var onDate = DateTime.Now;

            foreach (var course in await _context.Courses.Where(x => x.IsActive).Include(x => x.Image).Include(x => x.Chapters).OrderBy(x => x.Order).ToListAsync(cancellationToken))
            {
                var completedChapters = userAnswers.Where(x =>
                    course.Chapters.Any(y => y.Id == x.ChapterId && y.Answer == x.Answer)).ToList();
                var completedChaptersCount = completedChapters.Count();
                var chaptersCount = course.Chapters.Count();

                result.Add(new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Text = course.Text,
                    Order = course.Order,
                    ImageUrl = course.Image.Url,
                    CompletedChaptersCount = completedChaptersCount,
                    ChaptersCount = chaptersCount,
                    IsAvailable = isNextCourseAvailable
                });

                if (isNextCourseAvailable)
                {
                    if (completedChaptersCount == chaptersCount)
                    {
                        var firstAnswer = completedChapters.OrderBy(x => x.Created).First();
                        var lastAnswer = completedChapters.OrderBy(x => x.LastModified ?? x.Created).Last();

                        if (onDate.AddDays(-7).Date < firstAnswer.Created.ToLocalTime().Date
                            || onDate.AddDays(-1).Date < (lastAnswer.LastModified ?? lastAnswer.Created).ToLocalTime().Date)
                            isNextCourseAvailable = false;
                    }
                    else
                        isNextCourseAvailable = false;
                }
            }

            return result;
        }

        public async Task<CourseDetailsDto> GetDetailsAsync(string userId, int courseId, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.Where(x => x.Id == courseId).Include(x => x.Image).Include(x => x.Chapters).ThenInclude(x => x.Subchapters).SingleAsync();

            var chapters = new List<ChapterDto>();

            foreach (var chapter in course.Chapters.OrderBy(x => x.Order))
            {
                var userAnswer = await _context.UserAnswers.SingleOrDefaultAsync(x => x.UserId == userId && x.ChapterId == chapter.Id, cancellationToken);

                chapters.Add(new ChapterDto
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    Text = chapter.Text,
                    Order = chapter.Order,
                    ImageUrl = (await _context.Images.Where(x => x.Id == chapter.ImageId).SingleAsync(cancellationToken)).Url,
                    Subchapters = chapter.Subchapters.Select(x => new SubchapterDto
                    {
                        Title = x.Title,
                        Text = x.Text,
                        Order = x.Order,
                    }).OrderBy(x => x.Order).ToList(),
                    QuestionTitle = chapter.QuestionTitle,
                    QuestionText = chapter.QuestionText,
                    AnswerText1 = chapter.AnswerText1,
                    AnswerText2 = chapter.AnswerText2,
                    Answer = chapter.Answer,
                    UserAnswer = userAnswer?.Answer
                });
            }

            return new CourseDetailsDto
            {
                Title = course.Title,
                Text = course.Text,
                Order = course.Order,
                ImageUrl = course.Image.Url,
                Chapters = chapters
            };
        }

        public async Task InsertAnswerAsync(string userId, int courseId, AnswerPayload payload, CancellationToken cancellationToken)
        {
            var userAnswer = await _context.UserAnswers.SingleOrDefaultAsync(x => x.UserId == userId && x.ChapterId == payload.ChapterId, cancellationToken);

            if (userAnswer == null)
            {
                await _context.UserAnswers.AddAsync(new UserAnswer { UserId = userId, ChapterId = payload.ChapterId, Answer = payload.UserAnswer }, cancellationToken);
            }
            else
            {
                userAnswer.Answer = payload.UserAnswer;
                _context.UserAnswers.Update(userAnswer);
            }

            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                await CheckCoursesCompleted(userId, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + e.StackTrace);
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message + e.InnerException.StackTrace);
                }

                throw;
            }
        }

        private async Task CheckCoursesCompleted(string userId, CancellationToken cancellationToken)
        {
            var userCourses = await GetAsync(userId, cancellationToken);

            if (userCourses.All(x => x.CompletedChaptersCount == x.ChaptersCount))
            {
                bool sendEmail = false;
                var purchaseDate = DateTime.MinValue;

                var appStoreZPPSubscription = await _context.AppStoreSubscriptions.SingleOrDefaultAsync(x => x.UserId == userId && x.Type == SubscriptionType.ZPP, cancellationToken);

                if (appStoreZPPSubscription != null && appStoreZPPSubscription.IsValid)
                {
                    var validateReceiptValidationResult = await _appStoreClient.ValidateReceipt(SubscriptionType.ZPP, appStoreZPPSubscription.ReceiptData);
                    sendEmail = validateReceiptValidationResult.PurchaseDate.Value.AddMonths(6) > DateTime.Now;
                    purchaseDate = validateReceiptValidationResult.PurchaseDate.Value;
                }
                else
                {
                    var googlePlayStoreZPPSubscription = await _context.GooglePlayStoreSubscriptions.SingleOrDefaultAsync(x => x.UserId == userId && x.Type == SubscriptionType.ZPP, cancellationToken);

                    if (googlePlayStoreZPPSubscription != null && googlePlayStoreZPPSubscription.IsValid)
                    {
                        var validateReceiptValidationResult = await _googlePlayStoreClient.ValidatePurchase(SubscriptionType.ZPP, googlePlayStoreZPPSubscription.SubscriptionId, googlePlayStoreZPPSubscription.PurchaseToken);
                        sendEmail = validateReceiptValidationResult.PurchaseDate.Value.AddMonths(6) > DateTime.Now;
                        purchaseDate = validateReceiptValidationResult.PurchaseDate.Value;
                    }
                }

                if (sendEmail)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

                    byte[] bytes = null;

                    using (WebClient client = new WebClient())
                    {
                        var template = client.DownloadData(_templateUrl);

                        var endDateText = DateTime.Now.ToString("dd.MM.yyyy");
                        var purchaseDateText = purchaseDate.ToString("dd.MM.yyyy");

                        bytes = _pdfService.ReplaceText(template, "xx.mm.yyyy", endDateText);
                        bytes = _pdfService.ReplaceText(bytes, "xx-von.mm.yyyy", purchaseDateText);
                        bytes = _pdfService.ReplaceText(bytes, "xx-bis.mm.yyyy", endDateText);
                    }

                    Stream bodyTemplateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{this.GetType().Namespace}.CoursesEmailTemplate.html");

                    if (bodyTemplateStream == null)
                    {
                        throw new ArgumentNullException(nameof(bodyTemplateStream));
                    }

                    StreamReader reader = new StreamReader(bodyTemplateStream);
                    string body = reader.ReadToEnd();

                    var result = await _emailService.SendEmailAsync(
                        user.Email,
                        null,
                        "Teilnahmebescheinigung myFoodDoctor Kurs \"Iss dich schlank und gesund\"",
                        body,
                        new[] {
                            new Attachment()
                            {
                                Content = bytes,
                                Filename = "teilnahmebescheinigung-praeventionskurs.pdf",
                                Type = "application/pdf"
                            }
                        });

                    if (!result)
                    {
                        throw new Exception($"Unable to send an email to {user.Email}");
                    }
                }
            }
        }
    }
}

