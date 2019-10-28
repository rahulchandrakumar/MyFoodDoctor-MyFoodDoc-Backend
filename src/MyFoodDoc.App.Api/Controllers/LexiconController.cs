using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.Api.Models;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class LexiconController : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LexiconShallowEntry>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(
                new[] {
                    new LexiconShallowEntry
                    {
                        Id = 1,
                        Title = "Eiweiß",
                    },
                    new LexiconShallowEntry
                    {
                        Id = 2,
                        Title = "Proteine",
                    },
                }
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LexiconEntry), StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromRoute] int id)
        {
            if (id == 1)
            {
                return Ok(new LexiconEntry
                {
                    Title = "Eiweiß",
                    ImageUrl = "https://picsum.photos/375/150",
                    Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                });
            }
            else
            {
                return Ok(new LexiconEntry
                {
                    Title = "Proteine",
                    ImageUrl = "https://picsum.photos/375/150",
                    Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                });
            }
        }
    }
}