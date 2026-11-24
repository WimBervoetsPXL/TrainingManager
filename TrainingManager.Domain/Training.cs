using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingManager.Domain
{
    public class Training
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TrainerName { get; set; }
        public int DurationInHours { get; set; }

        public Training()
        {
            
        }

        public Training(int id, string title, string trainerName, int durationInHours)
        {
            Id = id;
            Title = title;
            TrainerName = trainerName;
            DurationInHours = durationInHours;
        }
    }
}
