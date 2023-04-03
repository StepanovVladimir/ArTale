using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class PursuitFunction : ICharacterFunction
    {
        public PursuitFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (characters.Hero.Equals("Иван"))
                {
                    StorylineString = "Змеиха летит в погоню за Иваном и царевной";
                }
                else if (characters.Hero.Equals("Фролка"))
                {
                    StorylineString = "Змеиха летит в погоню за Фролкой и царевной";
                }
                else if (characters.Hero.Equals("Зорька"))
                {
                    StorylineString = "Змеиха летит в погоню за Зорькой и царевной";
                }
                else
                {
                    StorylineString = "Змеиха летит в погоню за Никитой Кожемякой и царевной";
                }
                ActingCharacter = characters.Antagonist2;
                AdditionalCharacter2 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                StorylineString = "Гуси-лебеди летят в погоню за сестрой и братцем";
                ActingCharacter = characters.Antagonist1;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                if (characters.Hero.Equals("Ивашко"))
                {
                    StorylineString = "Баба-яга преследует Ивашку";
                }
                else
                {
                    StorylineString = "Баба-яга преследует Терешечку";
                }
                ActingCharacter = characters.Antagonist1;
            }
        }

        public string Name { get; } = "Преследование";

        public string StorylineString { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
