using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Validation;
using NzbDrone.Core.Validation.Paths;
using TidalSharp.Data;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class TidalIndexerSettingsValidator : AbstractValidator<TidalIndexerSettings>
    {
        public TidalIndexerSettingsValidator()
        {
            RuleFor(x => x.ConfigPath).IsValidPath();
        }
    }

    public class TidalIndexerSettings : IIndexerSettings
    {
        private static readonly TidalIndexerSettingsValidator Validator = new TidalIndexerSettingsValidator();

        [FieldDefinition(0, Label = "RedirectUrl", Type = FieldType.Textbox)]
        public string RedirectUrl { get; set; } = "";
        [FieldDefinition(1, Label = "Config Path", Type = FieldType.Textbox, HelpLink = "This is the directory where you account's information is stored so that it can be reloaded later.")]
        public string ConfigPath { get; set; } = "";

        [FieldDefinition(2, Label = "Audio Download Quality", Type = FieldType.Number)] // TODO: i have no idea how to display an enum list
        public AudioQuality AudioQuality { get; set; } = AudioQuality.HI_RES;           // TODO: it should be possible to make it instead list all available qualities? this was just how tidal-dl-ng does it but i don't think it matters

        [FieldDefinition(3, Type = FieldType.Number, Label = "Early Download Limit", Unit = "days", HelpText = "Time before release date Lidarr will download from this indexer, empty is no limit", Advanced = true)]
        public int? EarlyReleaseLimit { get; set; }

        // this is hardcoded so this doesn't need to exist except that it's required by the interface
        public string BaseUrl { get; set; } = "";

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
