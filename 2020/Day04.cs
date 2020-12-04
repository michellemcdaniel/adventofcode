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
            List<Passport> passports = new List<Passport>();
            bool createNew = true;
            
            foreach (string line in inputPassports)
            {
                if (string.IsNullOrEmpty(line))
                {
                    createNew = true;
                    continue;
                }

                Passport passport;
                if (createNew)
                {
                    passport = new Passport();
                }
                else
                {
                    passport = passports.Last();
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
                if (createNew)
                {
                    passports.Add(passport);
                }

                createNew = false;
            }

            int validPassports = 0;

            foreach(Passport passport in passports)
            {
                if (!string.IsNullOrEmpty(passport.byr) &&
                    !string.IsNullOrEmpty(passport.iyr) &&
                    !string.IsNullOrEmpty(passport.eyr) &&
                    !string.IsNullOrEmpty(passport.hgt) &&
                    !string.IsNullOrEmpty(passport.hcl) &&
                    !string.IsNullOrEmpty(passport.ecl) &&
                    !string.IsNullOrEmpty(passport.pid) &&
                    passport.ValidatePassport())
                {
                    validPassports++;
                }
            }

            Console.WriteLine($"Valid Passports: {validPassports}");
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
            string pattern = "(?<height>[0-9]+)(?<unit>.*)";
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
            string pattern = @"#[0-9a-f]+";
            Match match = Regex.Match(hcl, pattern);
            return match.Success && hcl.Length == 7;
        }

        public bool ValidateEyeColor()
        {
            return ecl == "amb" || 
                ecl == "blu" ||
                ecl == "brn" ||
                ecl == "gry" ||
                ecl == "grn" ||
                ecl == "hzl" ||
                ecl == "oth";
        }

        public bool ValidatePassportId()
        {
            string pattern = @"[0-9]+";
            Match match = Regex.Match(pid, pattern);

            return match.Success && pid.Length == 9;
        }

        public bool ValidatePassport()
        {
            return ValidateBirthYear() &&
                ValidateExpirationYear() &&
                ValidateEyeColor() &&
                ValidateHairColor() &&
                ValidateHeight() &&
                ValidateIssueYear() &&
                ValidatePassportId();
        }
    }
}
