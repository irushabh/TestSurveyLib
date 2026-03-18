using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBuilder.Models
{
    public class SurveyResponse
    {
        public Guid QuestionId { get; set; }
        public string? TextAnswer { get; set; }
        public List<string> SelectedOptions { get; set; } = new();
        public int? RatingAnswer { get; set; }
        public bool? YesNoAnswer { get; set; }
    }
}
