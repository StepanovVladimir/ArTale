using Assets.Scripts.ProppMorphology.CharacterFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology
{
    class CharacterFunctionsSequenceGenerator
    {
        private Random rnd = new Random();

        public List<ICharacterFunction> Generate()
        {
            var characterFunctions = new List<ICharacterFunction>();

            TaleKind taleKind;
            string initialPlace = null;
            string antagonistsPlace = null;
            string intermediatePlace = "Лес";

            var characters = new Characters();

            bool swanGeeseAreAntagonist = false;
            if (rnd.Next(0, 2) == 1)
            {
                taleKind = TaleKind.SnakeKidnapsPrincess;
                initialPlace = "Царский сад";
                antagonistsPlace = "Логово трёхглавого змея";

                int randomValue = rnd.Next(0, 4);
                if (randomValue == 0)
                {
                    characters.Hero = "Иван";
                }
                else if (randomValue == 1)
                {
                    characters.Hero = "Фролка-сидень";
                }
                else if (randomValue == 2)
                {
                    characters.Hero = "Зорька";
                }
                else
                {
                    characters.Hero = "Никита кожемяка";
                }

                characters.Victim = "Царевна";
                characters.Parent = "Царь";
                characters.Antagonist1 = "Трёхглавый змей";
                characters.Antagonist2 = "Змеиха";
            }
            else
            {
                initialPlace = "Двор дома";
                antagonistsPlace = "Избушка на курьих ножках";
                if (rnd.Next(0, 2) == 1)
                {
                    taleKind = TaleKind.SisterSavesBrotherFromBabaYaga;
                    characters.Hero = "Сестра";
                    characters.Victim = "Братец";
                }
                else
                {
                    taleKind = TaleKind.BoyEscapesFromBabaYaga;
                    if (rnd.Next(0, 2) == 1)
                    {
                        characters.Hero = "Терешечка";
                    }
                    else
                    {
                        characters.Hero = "Ивашко";
                    }
                }

                characters.Parent = "Старик и старушка";
                if (rnd.Next(0, 2) == 1)
                {
                    characters.Antagonist1 = "Баба-яга";
                    characters.Antagonist2 = "Дочь бабы-яги";
                }
                else
                {
                    swanGeeseAreAntagonist = true;
                    characters.Antagonist1 = "Гуси-лебеди";
                    characters.Antagonist2 = "Баба-яга";
                }
            }

            characterFunctions.Add(new InitialSituationFunction(taleKind, initialPlace, characters));
            characterFunctions.Add(new VillainyFunction(taleKind, initialPlace, characters));

            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                characterFunctions.Add(new TroubleMessageFunction(taleKind, initialPlace, characters));
                characterFunctions.Add(new CounteractionDecisionFunction(taleKind, initialPlace, characters));
            }

            var departureFunction = new DepartureFunction(taleKind, antagonistsPlace, characters);
            characterFunctions.Add(departureFunction);

            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                if (rnd.Next(0, 2) == 1)
                {
                    departureFunction.Place = intermediatePlace;
                    characterFunctions.Add(new TestByDonorFunction { Place = intermediatePlace });
                    characterFunctions.Add(new PassingTestFunction { Place = intermediatePlace });
                    characterFunctions.Add(new MagicAgentTransferFunction { Place = intermediatePlace });
                }
                else if (rnd.Next(0, 2) == 1)
                {
                    departureFunction.Place = intermediatePlace;
                    characterFunctions.Add(new MagicAgentTransferFunction { Place = intermediatePlace });
                }
            }

            if (taleKind != TaleKind.BoyEscapesFromBabaYaga && rnd.Next(0, 2) == 1)
            {
                departureFunction.Place = intermediatePlace;
                characterFunctions.Add(new TravelGuideFunction { Place = antagonistsPlace });
            }
            else if (departureFunction.Place == intermediatePlace)
            {
                characterFunctions.Add(new DepartureFunction(taleKind, antagonistsPlace, characters));
            }

            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                characterFunctions.Add(new StruggleFunction(taleKind, antagonistsPlace, characters));
                characterFunctions.Add(new VictoryFunction(taleKind, antagonistsPlace, characters));

                characterFunctions.Add(new TroublesLiquidationFunction(taleKind, antagonistsPlace, characters));

                if (rnd.Next(0, 2) == 1)
                {
                    characterFunctions.Add(new PursuitFunction { Place = intermediatePlace, ActingCharacter = characters.Antagonist2, AdditionalCharacter1 = characters.Hero, AdditionalCharacter2 = characters.Victim });
                    characterFunctions.Add(new RescueFromPursuitFunction { Place = "Кузница", ActingCharacter = "Кузнец", AdditionalCharacter1 = characters.Hero, AdditionalCharacter2 = characters.Antagonist2 });
                }
            }
            else
            {
                bool hasBeenStruggle = false;
                if (rnd.Next(0, 2) == 1)
                {
                    hasBeenStruggle = true;
                    characterFunctions.Add(new StruggleFunction(taleKind, antagonistsPlace, characters));
                    characterFunctions.Add(new VictoryFunction(taleKind, antagonistsPlace, characters));
                }

                bool addingPursuit = false;
                if (rnd.Next(0, 2) == 1)
                {
                    addingPursuit = true;
                }
                else if (!hasBeenStruggle)
                {
                    if (rnd.Next(0, 2) == 1)
                    {
                        characterFunctions.Add(new StruggleFunction(taleKind, antagonistsPlace, characters));
                        characterFunctions.Add(new VictoryFunction(taleKind, antagonistsPlace, characters));
                    }
                    else
                    {
                        addingPursuit = true;
                    }
                }

                if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
                {
                    characterFunctions.Add(new TroublesLiquidationFunction(taleKind, antagonistsPlace, characters));
                }

                if (addingPursuit)
                {
                    characterFunctions.Add(new PursuitFunction { Place = intermediatePlace, ActingCharacter = characters.Antagonist1, AdditionalCharacter1 = characters.Hero, AdditionalCharacter2 = characters.Victim });
                    characterFunctions.Add(new RescueFromPursuitFunction { Place = intermediatePlace });
                }
            }

            string finalLocation;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                finalLocation = "Царский дворец";
            }
            else
            {
                finalLocation = "Дом";
            }

            characterFunctions.Add(new ReturnFunction { Place = finalLocation });

            if (taleKind == TaleKind.SnakeKidnapsPrincess && rnd.Next(0, 2) == 1)
            {
                characterFunctions.Add(new RewardingFunction { Place = finalLocation });
            }

            return characterFunctions;
        }
    }
}
