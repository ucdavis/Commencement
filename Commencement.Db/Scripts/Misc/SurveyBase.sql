/*
Survey field types, taken from SIMS, run the scirpt on sims to get the values

select 'insert into SurveyFieldTypes (name, hasoptions, filterable) values ('''+ name +''', '+ cast(hasoptions as char(1)) + ', ' + cast(filterable as char(1)) + ')'
from FormFieldTypes
*/
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Text Box', 0, 1)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Text Area', 0, 1)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Boolean', 0, 0)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Boolean/Other', 1, 0)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Radio Buttons', 1, 0)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Checkbox List', 1, 0)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Drop Down', 1, 1)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Date', 0, 1)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('No Answer', 0, 0)
insert into SurveyFieldTypes (name, hasoptions, filterable) values ('Group Header', 0, 0)


/*
Regex validators
*/
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Required', 'required', '^.+$', '{0} is a required field.')
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Email', 'email', '(^((([a-z]|\d|[!#\$%&''\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&''\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$){1}|^$', '{0} is not a valid email.')
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Url', 'url', '(^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)|\/|\?)*)?$){1}|^$', '{0} is not a valid url.')
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Date', 'date', '(^(((0?[1-9]|1[012])[-\/](0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])[-\/](29|30)|(0?[13578]|1[02])[-\/]31)[-\/](19|[2-9]\d)\d{2}|0?2[-\/]29[-\/]((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$){1}|^$', '{0} is not a valid date.')
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Phone Number', 'phoneUS', '(^\(?[\d]{3}\)?[\s-]?[\d]{3}[\s-]?[\d]{4}$){1}|^$', '{0} is not a valid phone number.')
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Zip Code', 'zipUS', '^\d{5}(-\d{4})?$', '{0} is not a valid zip code.')




/*
Insert the initial survey info, replace 15 with the proper field id
*/
insert into SurveyFieldOptions (name, surveyfieldid) values ('Agricultural Management & Rangeland Resources', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Agricultural & Environmental Education', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Biology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Science', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Science & Management', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Atmospheric Science', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Avian Sciences', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Biotechnology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Clinical Nutrition', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Community & Regional Development', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Crop Science & Management', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Ecological Management & Restoration', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Entomology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Biology & Management', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Horticulture & Urban Forestry', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Policy, Analysis, & Planning', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental & Resource Sciences', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Science & Management', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Toxicology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Fiber & Polymer Science', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Food Science', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Human Development', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Hydrology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('International Agricultural Development', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Landscape Architecture', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Managerial Economics', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Nutrition Science', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Plant Sciences', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Sustainable Agriculture & Food Systems', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Textiles & Clothing', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Viticulture & Enology', 15)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Wildlife, Fish & Conservation Biology', 15)

insert into SurveyFieldOptions (name, surveyfieldid) values ('Job / Career in Agriculture (no further education)', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Job / Career in the Environmental Sciences (no further education)', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Job with a U.S. government agency', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('K-12 teaching', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Community college/university teaching', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Medicine', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Veterinary Medicine', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Pharmacy', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Dentistry', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Optometry', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Nursing School', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Physical Therapy', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Biotechnology', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Masters program', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Ph.D. program', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Masters program in another discipline', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Ph.D. program in another discipline', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Law school', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Business school', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Undecided', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Combination of the above (Please explain below)', 16)
insert into SurveyFieldOptions (name, surveyfieldid) values ('Other (Please explain below)', 16)

