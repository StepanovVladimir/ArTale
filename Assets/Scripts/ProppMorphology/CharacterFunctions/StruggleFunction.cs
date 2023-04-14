using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class StruggleFunction : ICharacterFunction
    {
        public StruggleFunction(TaleKind taleKind, string place, Characters characters)
        {
            SceneIndex = 6;
            Place = place;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (characters.Hero.Equals("Иван"))
                {
                    StorylineString = "Змей борется с Иваном";
                }
                else if (characters.Hero.Equals("Фролка"))
                {
                    StorylineString = "Змей борется с Фролкой";
                }
                else if (characters.Hero.Equals("Зорька"))
                {
                    StorylineString = "Змей борется с Зорькой";
                }
                else
                {
                    StorylineString = "Змей борется с Никитой Кожемякой";
                }
                ActingCharacter = characters.Antagonist1;
                AdditionalCharacter2 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                //StorylineString = "Баба-яга пытается посадить сестру и братца в печь";
                ActingCharacter = characters.Antagonist2;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                if (characters.Hero.Equals("Ивашко"))
                {
                    StorylineString = "Баба-яга велит своей дочери изжарить Ивашку в печи, а сама уходит";
                }
                else
                {
                    StorylineString = "Баба-яга велит своей дочери изжарить Терешечку в печи, а сама уходит";
                }
                ActingCharacter = characters.Antagonist2;
            }
        }

        public string Name { get; } = "Борьба";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
