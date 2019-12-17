namespace slmm.LawnMowing.Api.Factories
{
    using System;
    using Model;
    using Service;

    public class MowerFactory: IMowerFactory
    {
        private readonly ISettingsResolver settingsResolver;
        private Mower mowerInstance;

        public const string MowerStartupSettingsSectionName = "MowerStartup";
        public const string MowerXPosSettingsKeyName = "xPos";
        public const int MowerXPosSettingsDefaultValue = 1;
        public const string MowerYPosSettingsKeyName = "yPos";
        public const int MowerYPosSettingsDefaultValue = 1;
        public const string MowerOrientationSettingsKeyName = "orientation";
        public const string MowerOrientationSettingsDefaultValue = "East";
        public const string GardenLengthSettingsKeyName = "length";
        public const int GardenLengthSettingsDefaultValue = 1;
        public const string GardenWidthSettingsKeyName = "width";
        public const int GardenWidthSettingsDefaultValue = 1;
        public const string GardenDimentionsSettingsSectionName = "GardenDimentions";

        public MowerFactory(ISettingsResolver settingsResolver)
        {
            this.settingsResolver = settingsResolver;
        }

        public Mower Create()
        {
            if (this.mowerInstance == null)
            {
                var length = this.settingsResolver.ReadValue<int>(GardenDimentionsSettingsSectionName, GardenLengthSettingsKeyName, GardenLengthSettingsDefaultValue);
                var width = this.settingsResolver.ReadValue<int>(GardenDimentionsSettingsSectionName, GardenWidthSettingsKeyName, GardenWidthSettingsDefaultValue);
                var mower = new Mower(new Garden(length, width));


                var xPos = this.settingsResolver.ReadValue<int>(MowerStartupSettingsSectionName, MowerXPosSettingsKeyName, MowerXPosSettingsDefaultValue);
                var yPos = this.settingsResolver.ReadValue<int>(MowerStartupSettingsSectionName, MowerYPosSettingsKeyName, MowerYPosSettingsDefaultValue);
                var orientationString = this.settingsResolver.ReadValue(MowerStartupSettingsSectionName, MowerOrientationSettingsKeyName, MowerOrientationSettingsDefaultValue);

                if (Enum.TryParse<Orientation>(orientationString, out var orientation))
                {
                    mower.Start(new Position(new Coordinates(xPos, yPos), orientation));
                }

                this.mowerInstance = mower;
            }

            return this.mowerInstance;
        }
    }
}
