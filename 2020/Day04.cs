using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day04
    {
        public static void Execute()
        {
            List<string> inputPassports = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day04.txt")).ToList();

            Passport passport = new Passport();
            int validCheckedPassports = 0;
            int validUncheckedPassports = 0;
            
            foreach (string line in inputPassports)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (passport.ValidatePassport(false))
                    {
                        validUncheckedPassports++;
                    }
                    if (passport.ValidatePassport(true))
                    {
                        validCheckedPassports++;
                    }

                    passport = new Passport();
                    continue;
                }

                string[] data = line.Split(" ");
                
                foreach(string piece in data)
                {
                    string[] entry = piece.Split(":");
                    switch (entry[0])
                    {
                        case "byr":
                            passport.byr = entry[1];
                            break;
                        case "iyr":
                            passport.iyr = entry[1];
                            break;
                        case "eyr":
                            passport.eyr = entry[1];
                            break;
                        case "hgt":
                            passport.hgt = entry[1];
                            break;
                        case "hcl":
                            passport.hcl = entry[1];
                            break;
                        case "ecl":
                            passport.ecl = entry[1];
                            break;
                        case "pid":
                            passport.pid = entry[1];
                            break;
                        case "cid":
                            passport.cid = entry[1];
                            break;
                        default:
                            break;
                    }
                }
            }

            // Check the last one
            if (passport.ValidatePassport(false))
            {
                validUncheckedPassports++;
            }
            if (passport.ValidatePassport(true))
            {
                validCheckedPassports++;
            }

            Console.WriteLine($"Valid Unchecked Passports: {validUncheckedPassports}");
            Console.WriteLine($"Valid Checked Passports: {validCheckedPassports}");
        }
    }

    class Passport
    {
        public string byr { get; set; }
        public string iyr { get; set; }
        public string eyr { get; set; }
        public string hgt { get; set; }
        public string hcl { get; set; }
        public string ecl { get; set; }
        public string pid { get; set; }
        public string cid { get; set; }

        public bool ValidateBirthYear()
        {
            if(Int32.TryParse(byr, out int year))
            {
                return year >= 1920 && year <= 2002;
            }
            return false;
        }

        public bool ValidateIssueYear()
        {
            if(Int32.TryParse(iyr, out int year))
            {
                return year >= 2010 && year <= 2020;
            }
            return false;
        }

        public bool ValidateExpirationYear()
        {
            if(Int32.TryParse(eyr, out int year))
            {
                return year >= 2020 && year <= 2030;
            }
            return false;
        }

        public bool ValidateHeight()
        {
            string pattern = "(?<height>[0-9]+)(?<unit>in|cm)";
            Match match = Regex.Match(hgt, pattern);
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
            string pattern = "^#[0-9a-f]{6}$";
            return Regex.Match(hcl, pattern).Success;
        }

        public bool ValidateEyeColor()
        {
            string pattern = "^(amb|blu|brn|gry|grn|hzl|oth)$";
            return Regex.Match(ecl, pattern).Success;
        }

        public bool ValidatePassportId()
        {
            string pattern = "^[0-9]{9}$";
            return Regex.Match(pid, pattern).Success;
        }

        public bool ValidatePassport(bool validateContents)
        {
            bool allProvided = !string.IsNullOrEmpty(byr) &&
                !string.IsNullOrEmpty(iyr) &&
                !string.IsNullOrEmpty(eyr) &&
                !string.IsNullOrEmpty(hgt) &&
                !string.IsNullOrEmpty(hcl) &&
                !string.IsNullOrEmpty(ecl) &&
                !string.IsNullOrEmpty(pid);

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
