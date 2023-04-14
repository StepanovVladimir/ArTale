using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class VillainyFunction : ICharacterFunction
    {
        public VillainyFunction(TaleKind taleKind, string place, Characters characters)
        {
            SceneIndex = 1;
            Place = place;
            ActingCharacter = characters.Antagonist1;
            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                AdditionalCharacter1 = characters.Victim;
            }
            else
            {
                AdditionalCharacter1 = characters.Hero;
            }

            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = "Когда царевна отлучилась погулять в саду, змей прилетает и похищает её";
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                StorylineString = "Когда родители отлучились, гуси-лебеди налетают и похищают мальчика во дворе";
            }
            else
            {
                if (characters.Hero.Equals("Ивашко"))
                {
                    StorylineString = "Когда родители отлучились, Баба-яга похищает Ивашку, притворившись его мамой";
                }
                else
                {
                    StorylineString = "Когда родители отлучились, Баба-яга похищает Терешечку, притворившись его мамой";
                }
            }
        }

        public string Name { get; } = "Вредительство";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
