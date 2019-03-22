using AutoFixture.Xunit2;
using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.NSubstituteUtils;
using Xunit;

namespace Sitecore.NSubstitute.UnitTests
{
    public class FakeSectionTester
    {
        [Fact]
        public void Constructor_WhenCalled_CreatesSection()
        {
            TemplateSection section = new FakeTemplateSection();

            section.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void Constructor_WhenCalledWithFakeTemplate_SetsSectionTemplateToParent(ID templateId)
        {
            var template = new FakeTemplate(templateId: templateId);

            TemplateSection section = new FakeTemplateSection(template);

            section.Template.Should().Be(template.ToSitecoreTemplate());
        }

        [Fact]
        public void Constructor_WhenCalledWithoutName_SetsDefaultSectionName()
        {
            var template = new FakeTemplate();
            TemplateSection section = new FakeTemplateSection(template);

            section.Name.Should().Be("fakeSection");
        }


        [Theory, AutoData]
        public void Constructor_WhenCalledWithSectionId_SetsSectionId(ID sectionId)
        {            
            TemplateSection section = new FakeTemplateSection(new FakeTemplate(),sectionId: sectionId);

            section.ID.Should().Be(sectionId);            
        }

        [Theory, AutoData]
        public void Constructor_WhenCalledWithSectionName_SetsSectionName(string sectionName)
        {
            TemplateSection section = new FakeTemplateSection(new FakeTemplate(), sectionName: sectionName);
            
            section.Name.Should().Be(sectionName);
        }

        [InlineData("test icon")]
        [Theory, AutoData]
        public void WithIcon_WhenCalled_SetsSectionIcon(string sectionIcon)
        {            
            TemplateSection section = new FakeTemplateSection(new FakeTemplate()).WithIcon(sectionIcon);

            section.Icon.Should().Be(sectionIcon);
        }

        [InlineData(42)]
        [Theory, AutoData]
        public void WithSortorder_WhenCalled_SetsSectionSortOrder(int sortorder)
        {
            TemplateSection section = new FakeTemplateSection(new FakeTemplate())
              .WithSortorder(sortorder);

            section.Sortorder.Should().Be(sortorder);
        }

        [Fact]
        public void GetFields_WhenNothingWasAdded_ReturnsEmptySequence()
        {
            TemplateSection section = new FakeTemplateSection(new FakeTemplate());

            var sectionFields = section.GetFields();

            sectionFields.Should().BeEmpty();            
        }

        [Theory, AutoData]
        public void AddField_WhenFieldIdIsGiven_CreatesFieldWithId(ID fieldId, string fieldName)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            TemplateField field = section.AddField(fieldName, fieldId);

            field.ID.Should().Be(fieldId);
        }

        [Theory, AutoData]
        public void AddField_WhenFieldNameIsGiven_CreatesFieldWithName(ID fieldId, string fieldName)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            TemplateField field = section.AddField(fieldName, fieldId);

            field.Name.Should().Be(fieldName);
        }

        [Theory, AutoData]
        public void GetFieldById_WhenFieldWasAdded_FindsField(ID fieldId, string fieldName)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            TemplateField field = section.AddField(fieldName, fieldId);

            var actualField = section.ToSitecoreTemplateSection().GetField(fieldId);

            actualField.Should().BeSameAs(field);
        }

        [Theory, AutoData]
        public void GetFieldByName_WhenFieldWasAdded_FindsField(ID fieldId, string fieldName)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            TemplateField field = section.AddField(fieldName, fieldId);

            var actualField = section.ToSitecoreTemplateSection().GetField(fieldName);

            actualField.Should().BeSameAs(field);
        }


        [Theory, AutoData]
        public void GetFields_WhenFieldWasAdded_FindsField(ID fieldId, string fieldName)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            section.AddField(fieldName, fieldId);

            var actualFields = section.ToSitecoreTemplateSection().GetFields();

            actualFields
                .Should().ContainSingle(actualField => actualField.ID == fieldId)
                .And.HaveCount(1);
        }
        
        [Theory, AutoData]
        [InlineAutoData("test field name", 42)]
        public void WithSortOrderOnFieldLevel_WhenCalled_ReturnsConfiguredSortOrder(string fieldName, int sortOrder, ID fieldId)
        {
            var section = new FakeTemplateSection(new FakeTemplate());
            TemplateField field = section.AddField(fieldName, fieldId).WithSortorder(sortOrder);

            field.Sortorder.Should().Be(sortOrder);
        }
    }
}
