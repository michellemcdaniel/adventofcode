using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day04
    {
        public static void Execute(string filename)
        {
            List<string> inputPassports = File.ReadAllLines(filename).ToList();
            inputPassports.Add("");

            Passport passport = new Passport();
            int validatedPassports = 0;
            int checkedPassports = 0;
            
            foreach (string line in inputPassports)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (passport.ValidatePassport(false))
                    {
                        checkedPassports++;
                    }
                    if (passport.ValidatePassport(true))
                    {
                        validatedPassports++;
                    }

                    passport = new Passport();
                    continue;
                }

                string[] data = line.Split(" ");
                
                foreach(string entry in data)
                {
                    RegexHelper.Match(entry, @"^(.*):(.*)$", out string field, out string value);
                    switch (field)
                    {
                        case "byr":
                            passport.BirthYear = value;
                            break;
                        case "iyr":
                            passport.IssueYear = value;
                            break;
                        case "eyr":
                            passport.ExpirationYear = value;
                            break;
                        case "hgt":
                            passport.Height = value;
                            break;
                        case "hcl":
                            passport.HairColor = value;
                            break;
                        case "ecl":
                            passport.EyeColor = value;
                            break;
                        case "pid":
                            passport.PassportId = value;
                            break;
                        case "cid":
                            passport.CountryId = value;
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine($"Passports with all required fields: {checkedPassports}");
            Console.WriteLine($"Valid Passports: {validatedPassports}");
        }

        class Passport
        {
            public string BirthYear { get; set; }
            public string IssueYear { get; set; }
            public string ExpirationYear { get; set; }
            public string Height { get; set; }
            public string HairColor { get; set; }
            public string EyeColor { get; set; }
            public string PassportId { get; set; }
            public string CountryId { get; set; }

            public bool ValidateBirthYear()
            {
                if(Int32.TryParse(BirthYear, out int year))
                {
                    return year >= 1920 && year <= 2002;
                }
                return false;
            }

            public bool ValidateIssueYear()
            {
                if(Int32.TryParse(IssueYear, out int year))
                {
                    return year >= 2010 && year <= 2020;
                }
                return false;
            }

            public bool ValidateExpirationYear()
            {
                if(Int32.TryParse(ExpirationYear, out int year))
                {
                    return year >= 2020 && year <= 2030;
                }
                return false;
            }

            public bool ValidateHeight()
            {
                bool success = RegexHelper.Match(Height, @"(\d+)(in|cm)", out int height, out string units);
                if (success)
                {
                    if (units == "in")
                    {
                        return height >= 59 && height <= 76;
                    }
                    else if (units == "cm")
                    {
                        return height >= 150 && height <= 193;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            
            public bool ValidateHairColor()
            {
                return RegexHelper.Match(HairColor, "^#[0-9a-f]{6}$");
            }

            public bool ValidateEyeColor()
            {
                return RegexHelper.Match(EyeColor, "^(amb|blu|brn|gry|grn|hzl|oth)$", out string color);
            }

            public bool ValidatePassportId()
            {
                return RegexHelper.Match(PassportId, "^[0-9]{9}$");
            }

            public bool ValidatePassport(bool validateContents)
            {
                bool allProvided = !string.IsNullOrEmpty(BirthYear) &&
                    !string.IsNullOrEmpty(IssueYear) &&
                    !string.IsNullOrEmpty(ExpirationYear) &&
                    !string.IsNullOrEmpty(Height) &&
                    !string.IsNullOrEmpty(HairColor) &&
                    !string.IsNullOrEmpty(EyeColor) &&
                    !string.IsNullOrEmpty(PassportId);

                if (validateContents)
                {
                    return allProvided &&
                        ValidateBirthYear() &&
                        ValidateExpirationYear() &&
                        ValidateEyeColor() &&
                        ValidateHairColor() &&
                        ValidateHeight() &&
                        ValidateIssueYear() &&
                        ValidatePassportId();
                }

                return allProvided;
            }
        }
    }
}
