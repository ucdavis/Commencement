CREATE TABLE [dbo].[SurveyFieldXSurveyFieldValidators] (
    [SurveyFieldId]          INT NOT NULL,
    [SurveyFieldValidatorId] INT NOT NULL,
    CONSTRAINT [FK_SurveyFieldXSurveyFieldValidators_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [dbo].[SurveyFields] ([Id]),
    CONSTRAINT [FK_SurveyFieldXSurveyFieldValidators_SurveyFieldValidators] FOREIGN KEY ([SurveyFieldValidatorId]) REFERENCES [dbo].[SurveyFieldValidators] ([id])
);


