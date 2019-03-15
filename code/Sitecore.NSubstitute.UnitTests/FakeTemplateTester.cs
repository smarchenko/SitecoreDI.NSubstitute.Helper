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
            Template template = new FakeTemplate().WithBaseIDs(new [] { baseId });

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

        [Fact]
        public void FakeTemplate_Descendants()
        {
            string baseIDs = ID.NewID.ToString();
            var template = new FakeTemplate()
              .WithBaseIDs(new ID[] { new ID(baseIDs) }).ToSitecoreTemplate();

            template.DescendsFrom(new ID(baseIDs)).Should().BeTrue();
            template.DescendsFrom(ID.NewID).Should().BeFalse();
            template.DescendsFromOrEquals(new ID(baseIDs)).Should().BeTrue();
            template.DescendsFromOrEquals(ID.NewID).Should().BeFalse();
            template.DescendsFromOrEquals(template.ID).Should().BeTrue();
        }

        [Fact]
        public void FakeTemplate_SetFullName()
        {
            string fullName = "fake full name";
            var template = new FakeTemplate()
              .WithFullName(fullName);

            template.ToSitecoreTemplate().FullName.Should().Be(fullName);
            template.TemplateEngine.GetTemplate(fullName).Should().Be(template.ToSitecoreTemplate());
        }

        [Fact]
        public void FakeTemplate_SetIcon()
        {
            string icon = "some fake icon";
            var template = new FakeTemplate()
              .WithIcon(icon).ToSitecoreTemplate();

            template.Icon.Should().Be(icon);
        }

        [Fact]
        public void FakeTemplate_SetStandardValues()
        {
            ID svId = ID.NewID;
            var template = new FakeTemplate().WithStandatdValues(svId);

            template.ToSitecoreTemplate().StandardValueHolderId.Should().Be(svId);
        }

        [Fact]
        public void FakeTemplate_AddSection()
        {
            string name = "test name";
            ID id = ID.NewID;

            var fakeTemplate = new FakeTemplate();
            var section = fakeTemplate.AddSection(name, id);

            fakeTemplate.ToSitecoreTemplate().GetSections().Length.Should().Be(1);
            fakeTemplate.ToSitecoreTemplate().GetSection(id).Should().Be(section.ToSitecoreTemplateSection());
        }

        [Fact]
        public void FakeTemplate_AddField()
        {
            string name1 = "name1";
            string name2 = "name2";
            string name3 = "name3";
            string sectionName1 = "section1";
            string sectionName2 = "section2";

            var template = new FakeTemplate();
            template.AddField(sectionName1, name1, ID.NewID);
            template.AddField(sectionName1, name2, ID.NewID);
            template.AddField(sectionName2, name3, ID.NewID);

            template.ToSitecoreTemplate().GetSections().Length.Should().Be(2);
            template.ToSitecoreTemplate().GetSection(sectionName1).GetFields().Length.Should().Be(2);
            template.ToSitecoreTemplate().GetSection(sectionName2).GetFields().Length.Should().Be(1);

            template.ToSitecoreTemplate().GetFields().Length.Should().Be(3);
            template.ToSitecoreTemplate()
              .GetField(name2)
              .Section.Should()
              .Be(template.ToSitecoreTemplate().GetSection(sectionName1));
        }
    }
}
