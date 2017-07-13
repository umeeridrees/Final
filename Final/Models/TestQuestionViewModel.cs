using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class TestQuestionViewModel
    {
        public IEnumerable<Test> tests { get; set; }
        public IEnumerable<Question> questions { get; set; }
        public Test test { get; set; }
        public Question question { get; set; }
    }
}