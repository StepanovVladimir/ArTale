using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class TravelGuideFunction : ICharacterFunction
    {
        public TravelGuideFunction(TaleKind taleKind, string place, Characters characters, string helper)
        {
            Place = place;
            ActingCharacter = helper;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (characters.Hero.Equals("Иван"))
                {
                    StorylineString = "Старец указывает путь Ивану в логово трёхглавого змея";
                }
                else if (characters.Hero.Equals("Фролка"))
                {
                    StorylineString = "Старец указывает путь Фролке в логово трёхглавого змея";
                }
                else if (characters.Hero.Equals("Зорька"))
                {
                    StorylineString = "Старец указывает путь Зорьке в логово трёхглавого змея";
                }
                else
                {
                    StorylineString = "Старец указывает путь Никите Кожемяке в логово трёхглавого змея";
                }
            }
            else
            {
                StorylineString = "Ёжик указывает путь сестре к избушке на курьих ножках";
            }
        }

        public string Name { get; } = "Путеводительство";

        public string StorylineString { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
