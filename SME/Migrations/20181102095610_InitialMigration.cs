using Microsoft.EntityFrameworkCore.Migrations;

namespace Module_SME.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Concepts",
                columns: table => new
                {
                    ConceptId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concepts", x => x.ConceptId);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceId = table.Column<string>(nullable: false),
                    ResourceLink = table.Column<string>(nullable: false),
                    BloomLevel = table.Column<byte>(nullable: false),
                    HasPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    TechnologyId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.TechnologyId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<string>(nullable: false),
                    ProblemStatement = table.Column<string>(nullable: false),
                    BloomLevel = table.Column<byte>(nullable: false),
                    HasPublished = table.Column<bool>(nullable: false),
                    ResourceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceConcepts",
                columns: table => new
                {
                    ResourceId = table.Column<string>(nullable: false),
                    ConceptId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceConcepts", x => new { x.ConceptId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_ResourceConcepts_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "ConceptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceConcepts_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptTechnologies",
                columns: table => new
                {
                    ConceptId = table.Column<string>(nullable: false),
                    TechnologyId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptTechnologies", x => new { x.ConceptId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_ConceptTechnologies_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "ConceptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConceptTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningPlan",
                columns: table => new
                {
                    LearningPlanId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    HasPublished = table.Column<bool>(nullable: false),
                    TechnologyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPlan", x => x.LearningPlanId);
                    table.ForeignKey(
                        name: "FK_LearningPlan_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceTechnologies",
                columns: table => new
                {
                    ResourceId = table.Column<string>(nullable: false),
                    TechnologyId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTechnologies", x => new { x.ResourceId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_ResourceTechnologies_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptQuestions",
                columns: table => new
                {
                    ConceptId = table.Column<string>(nullable: false),
                    QuestionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptQuestions", x => new { x.ConceptId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ConceptQuestions_Concepts_ConceptId",
                        column: x => x.ConceptId,
                        principalTable: "Concepts",
                        principalColumn: "ConceptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConceptQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    IsCorrect = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<string>(nullable: true),
                    QuestionId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Options_Questions_QuestionId1",
                        column: x => x.QuestionId1,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    TopicId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LearningPlanId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_Topics_LearningPlan_LearningPlanId",
                        column: x => x.LearningPlanId,
                        principalTable: "LearningPlan",
                        principalColumn: "LearningPlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceTopics",
                columns: table => new
                {
                    ResourceId = table.Column<string>(nullable: false),
                    TopicId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTopics", x => new { x.ResourceId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_ResourceTopics_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConceptQuestions_QuestionId",
                table: "ConceptQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptTechnologies_TechnologyId",
                table: "ConceptTechnologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPlan_TechnologyId",
                table: "LearningPlan",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId1",
                table: "Options",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ResourceId",
                table: "Questions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceConcepts_ResourceId",
                table: "ResourceConcepts",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTechnologies_TechnologyId",
                table: "ResourceTechnologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceTopics_TopicId",
                table: "ResourceTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_LearningPlanId",
                table: "Topics",
                column: "LearningPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConceptQuestions");

            migrationBuilder.DropTable(
                name: "ConceptTechnologies");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "ResourceConcepts");

            migrationBuilder.DropTable(
                name: "ResourceTechnologies");

            migrationBuilder.DropTable(
                name: "ResourceTopics");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Concepts");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "LearningPlan");

            migrationBuilder.DropTable(
                name: "Technologies");
        }
    }
}
