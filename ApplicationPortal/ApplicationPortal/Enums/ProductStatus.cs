using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Enums
{
    public enum ProductStatus
    {
        [Display]
        GeneralInfoSubmitted,
        TechnicalInfoSubmitted,
        ExtraInfoSubmitted,
        FinallySubmitted,
        ApprovedByAdmin
    }
}