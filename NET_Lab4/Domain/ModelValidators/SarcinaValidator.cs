using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace NET_Lab4.Domain.ModelValidators
{
    public class SarcinaValidator : AbstractValidator<Sarcina>
    {
		public SarcinaValidator()
		{
			RuleFor(x => x.Id).NotNull();
			RuleFor(x => x.Deadline).GreaterThan(DateTime.Now);
			RuleFor(x => x.Stare).NotEqual(Stare.Closed);
			RuleFor(x => x.Importance).Equals(Importance.Medium);
		}
	}
}
