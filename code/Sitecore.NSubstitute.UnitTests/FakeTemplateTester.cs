using AutoFixture.Xunit2;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeTemplateTester
    {
        [Fact]
        public void Constructor_WhenCalledWithoutParameters_LeavesBaseIdsEmpty()
        {
            Template template = new FakeTemplate();

            template.BaseIDs.Should().BeEmpty();
        }

        [Fact]
        public void FakeTemplate_WhenCastedToSitecoreTemplate_ReturnsInstance()
        {
            Template template = new FakeTemplate();

            template.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WhenCalledWithoutParameters_LeavesDefaultTemplateName()
        {
            Template template = new FakeTemplate();

            template.Name.Should().Be("fakeTemplate");
        }

        [Theory, InlineAutoData("test template name"), AutoData]
        public void Constructor_WhenCalledWithName_SetsTemplateName(string templateName, ID id)
        {
            Template template = new FakeTemplate(templateName, id);

            template.Name.Should().Be(templateName);
        }

        [Theory, InlineAutoData("test template name"), AutoData]
        public void Constructor_WhenCalledWithId_SetsTemplateId(string templateName, ID id)
        {
            Template template = new FakeTemplate(templateName, id);

            template.ID.Should().Be(id);
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_SetsBaseIDs(ID baseId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.BaseIDs
                .Should().HaveCount(1)
                .And.ContainSingle(id => id == baseId);
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_FakesBaseTemplate(ID baseId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            var baseTemplates = template.GetBaseTemplates();

            baseTemplates
                .Should().HaveCount(1)
                .And.ContainSingle(baseTemplate => baseTemplate.ID == baseId);
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_ConfiguresDescendsFromBaseId(ID baseId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.DescendsFrom(baseId).Should().BeTrue();
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_ConfiguresDescendsFromOrEqualsBaseId(ID baseId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.DescendsFromOrEquals(baseId).Should().BeTrue();
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_ConfiguresDescendsFromOrEqualsForSelf(ID baseId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.DescendsFromOrEquals(template.ID).Should().BeTrue();
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_DoesNotDescendFrom(ID baseId, ID randomId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.DescendsFrom(randomId).Should().BeFalse();
        }

        [Theory, AutoData]
        public void WithBaseIDs_WhenCalled_DoesNotDescendFromOrEquals(ID baseId, ID randomId)
        {
            Template template = new FakeTemplate().WithBaseIDs(new[] { baseId });

            template.DescendsFromOrEquals(randomId).Should().BeFalse();
        }

        [AutoData]
        [Theory, InlineData("Fake template full name")]
        public void WithFullName_WhenCalled_SetsFullName(string fullName)
        {
            Template template = new FakeTemplate().WithFullName(fullName);

            template.FullName.Should().Be(fullName);
        }

        [AutoData]
        [Theory, InlineData("Fake template full name")]
        public void WithFullName_WhenCalled_ConfiguresTemplateEngineToFindTemplateByFullName(string fullName)
        {
            FakeTemplate template = new FakeTemplate().WithFullName(fullName);

            var foundByFullName = template.TemplateEngine.GetTemplate(fullName);

            foundByFullName.Should().Be(template.ToSitecoreTemplate());
        }

        [Theory, InlineData("some fake icon")]
        public void WithIcon_WhenCalled_SetsIcon(string icon)
        {
            Template template = new FakeTemplate().WithIcon(icon);

            template.Icon.Should().Be(icon);
        }

        [Theory, AutoData]
        public void WithStandatdValues_WhenCalled_SetsStandardValueHolderId(ID standardValuesHolderId)
        {
            Template template = new FakeTemplate().WithStandatdValues(standardValuesHolderId);

            template.StandardValueHolderId.Should().Be(standardValuesHolderId);
        }

        [AutoData]
        [Theory, InlineAutoData("test section name")]
        public void AddSection_WhenCalled_AddsSectionToParentTemplate(string name, ID id)
        {
            var fakeTemplate = new FakeTemplate();

            fakeTemplate.AddSection(name, id);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetSections().Should().ContainSingle();
        }

        [AutoData]
        [Theory, InlineAutoData("test section name")]
        public void AddSection_WhenCalled_AllowsGetSectionToBeFoundById(string name, ID id)
        {
            var fakeTemplate = new FakeTemplate();
            TemplateSection section = fakeTemplate.AddSection(name, id);

            Template template = fakeTemplate.ToSitecoreTemplate();

            var foundSection = template.GetSection(id);

            foundSection.Should().Be(section);
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalled_AddsFieldToTemplate(string fieldName, string sectionName, ID fieldId)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(sectionName, fieldName, fieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetFields().Should().ContainSingle(field => field.ID == fieldId);
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalled_AddsSection(string fieldName, string sectionName, ID fieldId)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(sectionName, fieldName, fieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetSections().Should().ContainSingle(section => section.Name == sectionName);
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalled_AddsFieldToSection(string fieldName, string sectionName, ID fieldId)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(sectionName, fieldName, fieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            var section = template.GetSection(sectionName);
                section.GetFields().Should().ContainSingle(field => field.ID == fieldId);
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalledTwoTimesForSameSection_CreatesOnlyOneSection(string unoField, string dosField, ID unoFieldId, ID dosFieldId, string sectionName)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(sectionName, unoField, unoFieldId);
            fakeTemplate.AddField(sectionName, dosField, dosFieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetSections().Should().ContainSingle();
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalledTwoTimesForSameSection_AddsToSameSection(string unoField, string dosField, ID unoFieldId, ID dosFieldId, string sectionName)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(sectionName, unoField, unoFieldId);
            fakeTemplate.AddField(sectionName, dosField, dosFieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            var section = template.GetSection(sectionName);
            section.GetFields()
                .Should().HaveCount(2)
                .And.ContainSingle(f => f.ID == unoFieldId, nameof(unoFieldId))
                .And.ContainSingle(f => f.ID == dosFieldId, nameof(dosFieldId));
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalledForDifferentSections_AddsTwoSections(string unoField, string dosField, ID unoFieldId, ID dosFieldId, string unoSectionName, string dosSectionName)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(unoSectionName, unoField, unoFieldId);
            fakeTemplate.AddField(dosSectionName, dosField, dosFieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetSections()
                .Should()
                .ContainSingle(section => section.Name == unoSectionName, nameof(unoSectionName))
                .And
                .ContainSingle(section => section.Name == dosSectionName, nameof(dosSectionName));
        }

        [AutoData]
        [Theory, InlineAutoData("fieldUno", "sectionUno")]
        public void AddField_WhenCalledForDifferentSections_AddsFieldsToDistinctSections(string unoField, string dosField, ID unoFieldId, ID dosFieldId, string unoSectionName, string dosSectionName)
        {
            var fakeTemplate = new FakeTemplate();
            fakeTemplate.AddField(unoSectionName, unoField, unoFieldId);
            fakeTemplate.AddField(dosSectionName, dosField, dosFieldId);

            Template template = fakeTemplate.ToSitecoreTemplate();

            template.GetSections()
                .Should()
                .ContainSingle(section => section.GetField(unoFieldId) != null, nameof(unoSectionName))
                .And
                .ContainSingle(section => section.GetField(dosFieldId) != null, nameof(dosSectionName));
        }
    }
}
