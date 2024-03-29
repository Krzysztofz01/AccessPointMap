﻿using FluentValidation;
using static AccessPointMap.Application.AccessPoints.Commands;
using static AccessPointMap.Application.AccessPoints.Commands.V1;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Validators
    {
        public static class V1
        {
            public class CreateValidator : AbstractValidator<Create>
            {
                public CreateValidator()
                {
                    RuleFor(c => c.ScanDate).NotNull();
                    RuleFor(c => c.AccessPoints).NotNull();
                    RuleForEach(c => c.AccessPoints).SetValidator(new AccessPointV1Validator());
                }
            }

            public class DeleteValidator : AbstractValidator<Delete>
            {
                public DeleteValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                }
            }

            public class DeleteRangeValidator : AbstractValidator<DeleteRange>
            {
                public DeleteRangeValidator()
                {
                    RuleFor(c => c.Ids).NotNull();
                    RuleForEach(c => c.Ids).NotNull();
                }
            }

            public class UpdateValidator : AbstractValidator<Update>
            {
                public UpdateValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.Note).NotNull();
                }
            }

            public class ChangeDisplayStatusValidator : AbstractValidator<ChangeDisplayStatus>
            {
                public ChangeDisplayStatusValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.Status).NotNull();
                }
            }

            public class ChangeDisplayStatusRangeValidator : AbstractValidator<ChangeDisplayStatusRange>
            {
                public ChangeDisplayStatusRangeValidator()
                {
                    RuleFor(c => c.Ids).NotNull();
                    RuleForEach(c => c.Ids).NotNull();
                    RuleFor(c => c.Status).NotNull();
                }
            }

            public class MergeWithStampValidator : AbstractValidator<MergeWithStamp>
            {
                public MergeWithStampValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.StampId).NotEmpty();
                    RuleFor(c => c.MergeLowSignalLevel).NotNull();
                    RuleFor(c => c.MergeHighSignalLevel).NotNull();
                    RuleFor(c => c.MergeSsid).NotNull();
                    RuleFor(c => c.MergeSecurityData).NotNull();
                }
            }

            public class DeleteStampValidator : AbstractValidator<DeleteStamp>
            {
                public DeleteStampValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.StampId).NotEmpty();
                }
            }

            public class CreateAdnnotationValidator : AbstractValidator<CreateAdnnotation>
            {
                public CreateAdnnotationValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.Title).NotEmpty();
                    RuleFor(c => c.Content).NotNull();
                }
            }

            public class DeleteAdnnotationValidator : AbstractValidator<DeleteAdnnotation>
            {
                public DeleteAdnnotationValidator()
                {
                    RuleFor(c => c.Id).NotEmpty();
                    RuleFor(c => c.AdnnotationId).NotEmpty();
                }
            }

            public class AccessPointV1Validator : AbstractValidator<Helpers.AccessPointV1>
            {
                public AccessPointV1Validator()
                {
                    RuleFor(m => m.Bssid).NotEmpty();
                    RuleFor(m => m.Ssid).NotNull();
                    RuleFor(m => m.Frequency).NotNull();
                    RuleFor(m => m.LowSignalLevel).NotNull();
                    RuleFor(m => m.LowSignalLatitude).NotNull();
                    RuleFor(m => m.LowSignalLongitude).NotNull();
                    RuleFor(m => m.HighSignalLevel).NotNull();
                    RuleFor(m => m.HighSignalLatitude).NotNull();
                    RuleFor(m => m.HighSignalLongitude).NotNull();
                    RuleFor(m => m.RawSecurityPayload).NotNull();
                }
            }
        }
    }
}
