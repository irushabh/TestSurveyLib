using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBuilder.Models
{
    public class SurveyQuestion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.Text;
        public bool IsRequired { get; set; }
        public List<string> Options { get; set; } = new(); // For MultipleChoice / Checkbox
        public int MaxRating { get; set; } = 5;            // For Rating
        public int MaxLength { get; set; } = 10;
    }
}
