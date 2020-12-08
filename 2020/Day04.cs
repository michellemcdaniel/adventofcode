using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                    string[] pair = entry.Split(":");
                    switch (pair[0])
                    {
                        case "byr":
                            passport.BirthYear = pair[1];
                            break;
                        case "iyr":
                            passport.IssueYear = pair[1];
                            break;
                        case "eyr":
                            passport.ExpirationYear = pair[1];
                            break;
                        case "hgt":
                            passport.Height = pair[1];
                            break;
                        case "hcl":
                            passport.HairColor = pair[1];
                            break;
                        case "ecl":
                            passport.EyeColor = pair[1];
                            break;
                        case "pid":
                            passport.PassportId = pair[1];
                            break;
                        case "cid":
                            passport.CountryId = pair[1];
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
                string pattern = "(?<height>[0-9]+)(?<unit>in|cm)";
                Match match = Regex.Match(Height, pattern);
                if (match.Success)
                {
                    int height = Int32.Parse(match.Groups["height"].Value);
                    if (match.Groups["unit"].Value == "in")
                    {
                        return height >= 59 && height <= 76;
                    }
                    else if (match.Groups["unit"].Value == "cm")
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
                return Regex.Match(HairColor, "^#[0-9a-f]{6}$").Success;
            }

            public bool ValidateEyeColor()
            {
                return Regex.Match(EyeColor, "^(amb|blu|brn|gry|grn|hzl|oth)$").Success;
            }

            public bool ValidatePassportId()
            {
                return Regex.Match(PassportId, "^[0-9]{9}$").Success;
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
