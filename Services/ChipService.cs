using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem.Services
{
    internal class ChipService
    {

        internal List<ColorChip> ValidCombinations(List<ColorChip> chips, Color startColor, Color endColor, bool smallestList)
        {
            List<List<ColorChip>> validCombinations = new List<List<ColorChip>>();
            List<ColorChip> startingChips = chips.Where(chip => chip.StartColor == startColor).ToList();
            foreach (ColorChip chip in startingChips)
            {
                List<ColorChip> combination = ProcessChips(chips, startColor, endColor, new List<ColorChip>(), smallestList);
                if (combination != null)
                    validCombinations.Add(combination);
            }

            return GetPreferredList(validCombinations, smallestList);
        }

        private List<ColorChip> ProcessChips(List<ColorChip> chips, Color currentColor, Color finalColor, List<ColorChip> chipPattern, bool smallestList)
        {
            //Finds the preferred list length for a branch of List<ColorChiops>
            List<List<ColorChip>> allCombinations = new List<List<ColorChip>>();
            if(currentColor == finalColor)
            {
                allCombinations.Add(chipPattern);
            }
            if (chips == null || !chips.Where(a=>a.StartColor == currentColor).Any() || !chips.Where(a=>a.EndColor == finalColor).Any())
            {
                return GetPreferredList(allCombinations, smallestList);
            }

            //Finds all valid chip combinations
            List<ColorChip> nextChips = chips.Where(a=>a.StartColor == currentColor).ToList();
            foreach (ColorChip chip in nextChips)
            {
                List<ColorChip> listMinusCurrent = ModifiedList(chips, chip, true);
                List<ColorChip> newChipPattern = ModifiedList(chipPattern, chip, false);
                List<ColorChip> combination = ProcessChips(listMinusCurrent, chip.EndColor, finalColor, newChipPattern, smallestList);
                if (combination != null)
                    allCombinations.Add(combination);
            }

            List<ColorChip> preferredList = GetPreferredList(allCombinations, smallestList); ;
            return preferredList;
        }

        private List<ColorChip> GetPreferredList(List<List<ColorChip>> allCombinations, bool smallestList)
        {
            if (allCombinations.Count() == 0)
                return null;
            if (smallestList)
                return allCombinations.Where(a => a.Count() == allCombinations.Min(b => b.Count())).FirstOrDefault();
            return allCombinations.Where(a => a.Count() == allCombinations.Max(b => b.Count())).FirstOrDefault();
        }

        private List<ColorChip> ModifiedList(List<ColorChip> source, ColorChip chipToRemove, bool removeChip)
        {
            // Null check to prevent exceptions
            if (source == null) return null;
            bool matchSkipped = false;
            List<ColorChip> list = new List<ColorChip>();
            foreach (var item in source)
            {
                if(removeChip && !matchSkipped && item == chipToRemove)
                {
                    matchSkipped = true;
                    continue; // Skip the first occurrence of the matching item
                }
                else
                {
                    list.Add(item);
                }
            }
            if(!removeChip) //Append chip to the end of the list if we are not removing it 
            {
                list.Add(chipToRemove);
            }
            return list;
        }

        internal List<ColorChip> GetTestChips()
        {
            List<ColorChip> chips = new List<ColorChip>();
            chips.Add(new ColorChip(Color.Purple, Color.Green));
            chips.Add(new ColorChip(Color.Orange, Color.Red));
            chips.Add(new ColorChip(Color.Blue, Color.Orange));
            chips.Add(new ColorChip(Color.Red, Color.Green));
            chips.Add(new ColorChip(Color.Green, Color.Purple));
            chips.Add(new ColorChip(Color.Yellow, Color.Orange));
            return chips;
        }

        internal List<ColorChip> GetTestRandomChips(int n)
        {
            Random rand = new Random();
            List<ColorChip> chips = new List<ColorChip>();
            for(int i = 0; i < n; i++)
            {
                Color firstColor = (Color)rand.Next(0, 5);
                Color secondColor = (Color)rand.Next(0, 5);

                chips.Add(new ColorChip(firstColor, secondColor));
            }
            return chips;
        }
    }
}
