namespace Ajka.Common.Constants.Service
{
    public class OrderEmailConstants
    {
        public const string emailFormatMatchRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public const string administratorEmail = "Frogger@centrum.cz";

        public const string errorEmailAddressIsNotValid = "Emailová adresa nemá platný formát!";
        public const string errorNoItemsInBasket = "Košík neobsahuje zboží!";
    }
}
