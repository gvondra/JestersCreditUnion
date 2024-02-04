using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationRatingLogLoader
    {
        private readonly LoanApplicationRatingLogVM _loanApplicationRatingLogVM;

        public LoanApplicationRatingLogLoader(LoanApplicationRatingLogVM loanApplicationRatingLogVM)
        {
            _loanApplicationRatingLogVM = loanApplicationRatingLogVM;
        }

        public void Load()
        {
            FlowDocument document = new FlowDocument();

            RatingVM ratingVM = _loanApplicationRatingLogVM?.LoanApplication?.Rating;
            if (ratingVM != null)
            {
                
                Paragraph paragraph = new Paragraph();
                document.Blocks.Add(paragraph);
                paragraph.Inlines.Add($"{ratingVM.Value:0.000} total loan application rating");
            }

            if (ratingVM?.InnerRating?.RatingLogs != null)
            {
                Paragraph paragraph = new Paragraph();
                document.Blocks.Add(paragraph);
                foreach (RatingLog ratingLog in ratingVM.InnerRating.RatingLogs)
                {
                    paragraph.Inlines.AddRange(ParseMessage(ratingLog.Description));
                    paragraph.Inlines.Add($" (score {ratingLog.Value:0.000})");
                    paragraph.Inlines.Add(new LineBreak());
                }
            }

            _loanApplicationRatingLogVM.Document=  document;
        }

        private static List<Inline> ParseMessage(string description)
        {
            List<Inline> inlines = new List<Inline>();
            Match match = Regex.Match(description, @"^(\[Pass\]|\[Fail\])?(.*)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
            if (match.Success)
            {
                for (int i = 1; i < match.Groups.Count; i += 1)
                {
                    if (string.Equals(match.Groups[i].Value, @"[Pass]", StringComparison.OrdinalIgnoreCase))
                    {
                        inlines.Add(new Run(match.Groups[i].Value.Trim()) { Foreground = Brushes.Green });
                        inlines.Add(new Run("\t"));
                    }
                    else if (string.Equals(match.Groups[i].Value, @"[Fail]", StringComparison.OrdinalIgnoreCase))
                    {
                        inlines.Add(new Run(match.Groups[i].Value.Trim()) { Foreground = Brushes.Red });
                        inlines.Add(new Run("\t"));
                    }
                    else
                    {
                        inlines.Add(new Run(match.Groups[i].Value.Trim()));
                    }
                }
            }
            return inlines;
        }
    }
}
