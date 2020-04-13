using GameProducer.Util;
using System.Collections.Generic;
using System.Linq;
using GameProducer.Domain.DTO;
using GameProducer.Domain.Model;
using GameProducer.Domain.Enum;
using GameProducer.Infrastructure.Extensions;

namespace GameProducer.Domain.Translators
{
    public static class EntityTranslator
    {
        public static Game ToDomain(this IGDBNextGameRelease release)
        {
            if (release != null)
            {
                System.Enum.TryParse(release.platform.abbreviation.ToLower(), true, out ConsoleType consoleType);
                return new Game()
                {
                    externalId = release.game.id,
                    name = release.game.name,
                    summary = release.game.summary,
                    publisher = GetPublisherName(release.game.involved_companies),
                    releaseDate = release.date.FromUnixTimeStampToDateTime(),
                    consoleType = consoleType,
                    consoleAbbreviation = release.platform.abbreviation.ToLower(),
                    consoleName = release.platform.name
                };
            }

            return null;
        }

        public static IList<Game> ToDomainList(this IEnumerable<IGDBNextGameRelease> releases)
        {
            return releases?.Select(ToDomain).ToList();
        }

        private static string GetPublisherName(IEnumerable<IGDBInvolvedCompany> involved_companies)
        {
            if (involved_companies.IsNullOrEmpty())
                return string.Empty;

            var publisher = involved_companies
                .Select(x => new { x.publisher, x.company })
                .Where(bundle => bundle.publisher)
                .FirstOrDefault();

            return publisher?.company.name;
        }
    }
}
