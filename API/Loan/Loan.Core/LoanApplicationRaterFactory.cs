using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using RateLoanApplication = JestersCreditUnion.Loan.Core.Rate.LoanApplication;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationRaterFactory : ILoanApplicationRaterFactory
    {
        private const string _configurationResourceName = "JestersCreditUnion.Loan.Core.Rate.LoanApplication.Configuration.json";
        private readonly IRatingFactory _ratingFactory;

        public LoanApplicationRaterFactory(IRatingFactory ratingFactory)
        {
            _ratingFactory = ratingFactory;
        }

        public Task<ILoanApplicationRater> Create()
        {
            RateLoanApplication.Aggregator aggregator = GetConfiguration();
            return Task.FromResult<ILoanApplicationRater>(
                new LoanApplicationRater(_ratingFactory, aggregator));
        }

        private RateLoanApplication.Aggregator GetConfiguration()
        {
            RateLoanApplication.Aggregator aggregator = new RateLoanApplication.Aggregator();
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_configurationResourceName);
            using StreamReader streamReader = new StreamReader(stream);
            using JsonTextReader jsonReader = new JsonTextReader(streamReader);
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.StartObject)
                {
                    RateLoanApplication.IComponent component = LoadComponent(jsonReader);
                    if (component != null)
                        aggregator.Add(component);
                }
            }
            return aggregator;
        }

        private RateLoanApplication.IComponent LoadComponent(JsonTextReader jsonReader)
        {
            LoanApplicationRatingComponent componentType = LoanApplicationRatingComponent.NotSet;
            int points = 0;
            string propertyName = string.Empty;
            bool exit = false;
            RateLoanApplication.IComponent result = null;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "ComponentType", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    componentType = (LoanApplicationRatingComponent)Convert.ToInt16(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "Points", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    points = Convert.ToInt32(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "Parameters", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.StartObject)
                {
                    result = CreateComponent(componentType, points, jsonReader);
                }
            }
            return result;
        }

        private RateLoanApplication.IComponent CreateComponent(LoanApplicationRatingComponent componentType, int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.IComponent result = null;
            switch (componentType)
            {
                case LoanApplicationRatingComponent.MinAge:
                    result = CreateMinAgeComponent(points, jsonReader);
                    break;
                case LoanApplicationRatingComponent.MaxAge:
                    result = CreateMaxAgeComponent(points, jsonReader);
                    break;
                case LoanApplicationRatingComponent.MinAmount:
                    result = CreateMinAmountComponent(points, jsonReader);
                    break;
                case LoanApplicationRatingComponent.MaxAmount:
                    result = CreateMaxAmountComponent(points, jsonReader);
                    break;
                case LoanApplicationRatingComponent.MinEmploymentYears:
                    result = CreateMinEmploymentYearsComponent(points, jsonReader);
                    break;
                case LoanApplicationRatingComponent.MaxEmploymentYears:
                    result = CreateMaxEmploymentYearsComponent(points, jsonReader);
                    break;
            }
            return result;
        }

        private RateLoanApplication.MaxAge CreateMaxAgeComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MaxAge component = new RateLoanApplication.MaxAge(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Maximum", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    component.Maximum = Convert.ToInt32(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "BorrowerAge", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.BorrowerAge = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "CoBorrowerAge", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.CoBorrowerAge = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }

        private RateLoanApplication.MinAge CreateMinAgeComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MinAge component = new RateLoanApplication.MinAge(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Minimum", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    component.Minimum = Convert.ToInt32(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "BorrowerAge", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.BorrowerAge = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "CoBorrowerAge", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.CoBorrowerAge = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }

        private RateLoanApplication.MaxAmount CreateMaxAmountComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MaxAmount component = new RateLoanApplication.MaxAmount(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Maximum", StringComparison.OrdinalIgnoreCase) && (jsonReader.TokenType == JsonToken.Float || jsonReader.TokenType == JsonToken.Integer))
                {
                    component.Maximum = Convert.ToDecimal(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }

        private RateLoanApplication.MinAmount CreateMinAmountComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MinAmount component = new RateLoanApplication.MinAmount(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Minimum", StringComparison.OrdinalIgnoreCase) && (jsonReader.TokenType == JsonToken.Float || jsonReader.TokenType == JsonToken.Integer))
                {
                    component.Minimum = Convert.ToDecimal(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }

        private RateLoanApplication.MaxEmploymentYears CreateMaxEmploymentYearsComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MaxEmploymentYears component = new RateLoanApplication.MaxEmploymentYears(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Maximum", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    component.Maximum = Convert.ToInt32(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "BorrowerEmployment", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.BorrowerEmployment = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "CoBorrowerEmployment", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.CoBorrowerEmployment = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }

        private RateLoanApplication.MinEmploymentYears CreateMinEmploymentYearsComponent(int points, JsonTextReader jsonReader)
        {
            RateLoanApplication.MinEmploymentYears component = new RateLoanApplication.MinEmploymentYears(_ratingFactory, points);
            bool exit = false;
            string propertyName = string.Empty;
            while (!exit && jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.EndObject)
                {
                    exit = true;
                }
                else if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "LogMessageTemplage", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.String)
                {
                    component.LogMessageTemplage = jsonReader.Value.ToString();
                }
                else if (string.Equals(propertyName, "Minimum", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Integer)
                {
                    component.Minimum = Convert.ToInt32(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "BorrowerEmployment", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.BorrowerEmployment = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(propertyName, "CoBorrowerEmployment", StringComparison.OrdinalIgnoreCase) && jsonReader.TokenType == JsonToken.Boolean)
                {
                    component.CoBorrowerEmployment = Convert.ToBoolean(jsonReader.Value, CultureInfo.InvariantCulture);
                }
            }
            return component;
        }
    }
}
