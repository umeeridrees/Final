using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class StudentTestResultViewModel
    {
        public List<Student> students { get; set; }
        public List<Test> tests { get; set; }
        public List<Take> Takes { get; set; }

        public Student student { get; set; }
        public Test test { get; set; }
        public Take take { get; set; }
    }
}