﻿namespace MyFoodDoc.App.Application.Models
{
    public class UserStatisticsDto
    {
        public bool IsZPPForbidden { get; set; }
        public bool HasSubscription { get; set; }
        public bool HasZPPSubscription { get; set; }
        public bool IsDiaryFull { get; set; }
        public bool HasNewTargetsTriggered { get; set; }
        public bool IsFirstTargetsEvaluation { get; set; }
        public bool HasTargetsActivated { get; set; }
        public int DaysTillFirstEvaluation { get; set; }
    }
}
