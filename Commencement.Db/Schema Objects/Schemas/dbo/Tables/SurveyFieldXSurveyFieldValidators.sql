CREATE TABLE [dbo].[SurveyFieldXSurveyFieldValidators]
(
	[SurveyFieldId]          INT NOT NULL,
    [SurveyFieldValidatorId] INT NOT NULL, 
    CONSTRAINT [FK_SurveyFieldXSurveyFieldValidators_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [SurveyFields]([Id]), 
    CONSTRAINT [FK_SurveyFieldXSurveyFieldValidators_SurveyFieldValidators] FOREIGN KEY ([SurveyFieldValidatorID]) REFERENCES [SurveyFieldValidators]([Id]),
)
