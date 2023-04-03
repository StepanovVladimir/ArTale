using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class RescueFromPursuitFunction : ICharacterFunction
    {
        public RescueFromPursuitFunction(TaleKind taleKind, string place, Characters characters, string helper)
        {
            Place = place;
            ActingCharacter = helper;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (characters.Hero.Equals("Иван"))
                {
                    StorylineString = "Кузнец в кузнице помогает Ивану справится со змеихой";
                }
                else if (characters.Hero.Equals("Фролка"))
                {
                    StorylineString = "Кузнец в кузнице помогает Фролке справится со змеихой";
                }
                else if (characters.Hero.Equals("Зорька"))
                {
                    StorylineString = "Кузнец в кузнице помогает Зорьке справится со змеихой";
                }
                else
                {
                    StorylineString = "Кузнец в кузнице помогает Никите Кожемяке справится со змеихой";
                }
                AdditionalCharacter2 = characters.Antagonist2;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                StorylineString = "Яблоня прячет сестру и братца от преследования гусей-лебедей";
                AdditionalCharacter2 = characters.Antagonist1;
            }
            else
            {
                if (characters.Hero.Equals("Ивашко"))
                {
                    StorylineString = "Гусь-лебедь спасает Ивашку от преследования бабы-яги, улетев с ним";
                }
                else
                {
                    StorylineString = "Гусь-лебедь спасает Терешечку от преследования бабы-яги, улетев с ним";
                }
                AdditionalCharacter2 = characters.Antagonist1;
            }
        }

        public string Name { get; } = "Спасение от преследования";

        public string StorylineString { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
