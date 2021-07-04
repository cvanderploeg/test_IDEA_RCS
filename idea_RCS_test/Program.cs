using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Concrete;
using IdeaRS.OpenModel.Concrete.Load;
using IdeaRS.OpenModel.Material;
using IdeaRS.OpenModel.CrossSection;
using IdeaRS.OpenModel.Geometry2D;
using IdeaRS.OpenModel.Model;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace test_IDEA_RCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start creating model");

            // start a new open model
            OpenModel openModel = new OpenModel();

            AddProjectData(openModel);
            ReinforcedCrossSection rcs = CreateReinforcedCrossSection(openModel);
            CheckMember checkMember = CheckMemberSettings(openModel);

            Console.WriteLine("start calculation");

            CalculateSections(openModel, rcs, checkMember);

            Console.WriteLine("finished");

        }

        static void AddProjectData(OpenModel openModel)
        {
            //Common project data
            var projectData = new ProjectData();
            projectData.Name = "Column project";
            projectData.Date = new DateTime(2019, 6, 4);

            //Additionl data for Ec
            var projectDataEc = new ProjectDataEc();
            projectDataEc.AnnexCode = NationalAnnexCode.Dutch;
            projectDataEc.FatigueCheck = false;
            projectDataEc.FatigueAnnexNN = false;
            projectData.CodeDependentData = projectDataEc;
            openModel.ProjectData = projectData;

            //Concrete project data
            var projectDataConcrete = new ProjectDataConcreteEc2();
            projectDataConcrete.CodeEN1992_2 = false;
            projectDataConcrete.CodeEN1992_3 = false;
            openModel.ProjectDataConcrete = projectDataConcrete;
        }

        static ReinforcedCrossSection CreateReinforcedCrossSection(OpenModel openModel)
        {
            //Concrete material
            MatConcreteEc2 mat = new MatConcreteEc2();
            mat.Name = "C30/37";
            mat.UnitMass = 2500.0;
            mat.E = 32836.6e6;
            mat.G = 13667000000.0;
            mat.Poisson = 0.2;
            mat.SpecificHeat = 0.6;
            mat.ThermalExpansion = 0.00001;
            mat.ThermalConductivity = 45;
            mat.Fck = 30.0e6;
            mat.CalculateDependentValues = true;
            openModel.AddObject(mat);

            //Reinforcement material
            MatReinforcementEc2 matR = new MatReinforcementEc2();
            matR.Name = "B 500B";
            matR.UnitMass = 7850.0;
            matR.E = 200e9;
            matR.Poisson = 0.2;
            matR.G = 83.333e9;
            matR.SpecificHeat = 0.6;
            matR.ThermalExpansion = 0.00001;
            matR.ThermalConductivity = 45;
            matR.Fyk = 500e6;
            matR.CoeffFtkByFyk = 1.08;
            matR.Epsuk = 0.025;
            matR.Type = ReinfType.Bars;
            matR.BarSurface = ReinfBarSurface.Ribbed;
            matR.Class = ReinfClass.B;
            matR.Fabrication = ReinfFabrication.HotRolled;
            matR.DiagramType = ReinfDiagramType.BilinerWithAnInclinedTopBranch;
            openModel.AddObject(matR);

            CrossSectionParameter css = new CrossSectionParameter(); //creating instance of cross-section defined by parameters
            css.Name = "CSS 1";
            css.Id = openModel.GetMaxId(css) + 1;
            css.CrossSectionType = CrossSectionType.Rect;
            css.Parameters.Add(new ParameterDouble() { Name = "Height", Value = 0.5 });
            css.Parameters.Add(new ParameterDouble() { Name = "Width", Value = 0.5 });
            css.Material = new ReferenceElement(mat);
            openModel.AddObject(css);

            //Reinforced section - concrete with reinforcement
            ReinforcedCrossSection rcs = new ReinforcedCrossSection();
            rcs.Name = "R 1";
            rcs.CrossSection = new ReferenceElement(css);
            openModel.AddObject(rcs);

            ReinforcedBar bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.1939;
            bar.Point.Y = 0.1939;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.1939;
            bar.Point.Y = -0.1939;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.1939;
            bar.Point.Y = -0.1939;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.1939;
            bar.Point.Y = 0.1939;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.0613;
            bar.Point.Y = -0.198;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.0613;
            bar.Point.Y = -0.198;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.0613;
            bar.Point.Y = 0.198;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.0613;
            bar.Point.Y = 0.198;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.198;
            bar.Point.Y = 0.0613;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = -0.198;
            bar.Point.Y = -0.0613;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.198;
            bar.Point.Y = -0.0613;
            rcs.Bars.Add(bar);

            bar = new ReinforcedBar();
            bar.Diameter = 0.020;
            bar.Material = new ReferenceElement(matR);
            bar.Point = new Point2D();
            bar.Point.X = 0.198;
            bar.Point.Y = 0.0613;
            rcs.Bars.Add(bar);

            var stirrup = new Stirrup();
            stirrup.Diameter = 0.012;
            stirrup.DiameterOfMandrel = 4.0;
            stirrup.Distance = 0.2;
            stirrup.IsClosed = true;
            stirrup.Material = new ReferenceElement(matR);
            var poly = new PolyLine2D();

            poly.StartPoint = new Point2D();
            poly.StartPoint.X = -0.214;
            poly.StartPoint.Y = 0.214;
            var segment = new LineSegment2D();
            segment.EndPoint = new Point2D();
            segment.EndPoint.X = -0.214;
            segment.EndPoint.Y = -0.214;
            poly.Segments.Add(segment);

            segment = new LineSegment2D();
            segment.EndPoint = new Point2D();
            segment.EndPoint.X = 0.214;
            segment.EndPoint.Y = -0.214;
            poly.Segments.Add(segment);

            segment = new LineSegment2D();
            segment.EndPoint = new Point2D();
            segment.EndPoint.X = 0.214;
            segment.EndPoint.Y = 0.214;
            poly.Segments.Add(segment);

            segment = new LineSegment2D();
            segment.EndPoint = new Point2D();
            segment.EndPoint.X = -0.214;
            segment.EndPoint.Y = 0.214;
            poly.Segments.Add(segment);

            stirrup.Geometry = poly;
            rcs.Stirrups.Add(stirrup);

            return rcs;
        }

        static CheckMember CheckMemberSettings(OpenModel openModel)
        { 
            var checkMember = new CheckMember1D(); //Design member data object
            openModel.AddObject(checkMember);

            //Concrete member data
            var memberData = new ConcreteMemberDataEc2(); //Member data base common object
            memberData.MemberType = ConcreteMemberType.Column;
            memberData.RelativeHumidity = 0.65;
            memberData.CreepCoeffInfinityValue = InputValue.Calculated;
            memberData.MemberImportance = MemberImportance.Major;

            memberData.ExposureClassesData = new ExposureClassesDataEc2(); //Exposure classes
            memberData.ExposureClassesData.NoCorrosionCheck = false;
            memberData.ExposureClassesData.CarbonationCheck = true;
            memberData.ExposureClassesData.Carbonation = ExposureClassEc2.XC3;
            memberData.ExposureClassesData.ChloridesCheck = true;
            memberData.ExposureClassesData.Chlorides = ExposureClassEc2.XD1;
            memberData.ExposureClassesData.ChloridesFromSeaCheck = false;
            memberData.ExposureClassesData.FreezeAttackCheck = false;
            memberData.ExposureClassesData.ChemicalAttackCheck = false;

            memberData.Element = new ReferenceElement(checkMember);
            openModel.AddObject(memberData);

            //Beam data are not necessary but must be created a default one
            memberData.BeamData = new BeamDataEc2();


            //Concrete member data
            memberData.ColumnData = new ColumnDataEc2();
            memberData.ColumnData.L = 3.0;
            memberData.ColumnData.EffectiveLength = InputValue.UserInput;
            memberData.ColumnData.L0Y = 3.0;
            memberData.ColumnData.L0Z = 3.0;

            memberData.ColumnData.SecondOrderEffectInput = InputValue.Calculated;
            memberData.ColumnData.GeometricImperfectionsULS = true;
            memberData.ColumnData.GeometricImperfectionsSLS = false;
            memberData.ColumnData.EffectConsidered = EffectConsideredType.IsolatedMember;
            memberData.ColumnData.ImperfectionDirection = ImperfectionDirection.FromSetup;

            memberData.ColumnData.Calculation2ndOrderEffect = true;
            memberData.ColumnData.BracedY = false;
            memberData.ColumnData.BracedZ = false;
            memberData.ColumnData.SecondOrderEffectMethod = SecondOrderEffectMethodEc2.NominalCurvature;
            memberData.ColumnData.ValueTypeOfcY = ValueTypec.UserDefined;
            memberData.ColumnData.UserValuecY = 9.8696;
            memberData.ColumnData.ValueTypeOfcZ = ValueTypec.UserDefined;
            memberData.ColumnData.UserValuecZ = 9.8696;


            // calculation settings
            memberData.CalculationSetup = new CalculationSetup();
            memberData.CalculationSetup.UlsDiagram = true;
            memberData.CalculationSetup.UlsShear = false;
            memberData.CalculationSetup.UlsTorsion = false;
            memberData.CalculationSetup.UlsInteraction = true;
            memberData.CalculationSetup.SlsStressLimitation = true;
            memberData.CalculationSetup.SlsCrack = true;
            memberData.CalculationSetup.Detailing = true;
            memberData.CalculationSetup.UlsResponse = true;
            memberData.CalculationSetup.SlsStiffnesses = false;
            memberData.CalculationSetup.MNKappaDiagram = false;

            //Concrete setup
            var setup = new ConcreteSetupEc2();
            setup.Annex = NationalAnnexCode.NoAnnex;
            openModel.ConcreteSetup = setup;

            return checkMember;
        }

        static void CalculateSections(OpenModel openModel, ReinforcedCrossSection rcs, CheckMember checkMember)
        {
            //Standard section
            var singleCheckSection = new StandardCheckSection();
            singleCheckSection.Description = "S 1";
            singleCheckSection.ReinfSection = new ReferenceElement(rcs);
            singleCheckSection.CheckMember = new ReferenceElement(checkMember);
            

            //add extreme to section
            var sectionExtreme = new StandardCheckSectionExtreme();
            sectionExtreme.Fundamental = new LoadingULS();
            sectionExtreme.Fundamental.InternalForces = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { N = -3750.0e3, My = 112.7e3, Mz = -52.0e3 };
            sectionExtreme.Fundamental.InternalForcesBegin = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { My = 22.0e3, Mz = -5.0e3 };
            sectionExtreme.Fundamental.InternalForcesEnd = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { My = 18.0e3, Mz = 10.0e3 };
            singleCheckSection.Extremes.Add(sectionExtreme);

            openModel.AddObject(singleCheckSection);


            //Creating instance of Rcs controller
            var rcsController = new IdeaStatiCa.RcsController.IdeaRcsController();
            System.Diagnostics.Debug.Assert(rcsController != null);
            //Assert.IsNotNull(rcsController);

            //Open rcs project from IOM
            IdeaRS.OpenModel.Message.OpenMessages messages;
            var ok = rcsController.OpenIdeaProjectFromIdeaOpenModel(openModel, "Column", out messages);
            System.Diagnostics.Debug.Assert(ok);

            string fileName = @"C:\Users\c.vd.ploeg\Desktop\idea\test.idea";
            rcsController.SaveAsIdeaProjectFile(fileName);

            //Calculate project
            ok = rcsController.Calculate(new List<int>() { singleCheckSection.Id });
            System.Diagnostics.Debug.Assert(ok);

            //gets the results
            var result = rcsController.GetResultOnSection(null);
            System.Diagnostics.Debug.Assert(result != null);

            // Storing to standard xml file
            XmlSerializer xs = new XmlSerializer(typeof(List<IdeaRS.OpenModel.Concrete.CheckResult.SectionConcreteCheckResult>));

            Stream fs = new FileStream(fileName, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            // Serialize using the XmlTextWriter.
            xs.Serialize(writer, result);
            writer.Close();
            fs.Close();

            var sectionResult = result.FirstOrDefault(it => it.SectionId == singleCheckSection.Id);
            System.Diagnostics.Debug.Assert(result != null);
            foreach (var extremeResult in sectionResult.ExtremeResults)
            {
                var overalResult = extremeResult.Overall;
                foreach (var check in overalResult.Checks)
                {
                    System.Diagnostics.Debug.WriteLine("{0} - {1} - {2}", check.ResultType, check.Result, check.CheckValue);
                }

                foreach (var checkResult in extremeResult.CheckResults)
                {
                    var checkType = checkResult.ResultType;
                    foreach (var checkResult1 in checkResult.CheckResults)
                    {
                        var res = checkResult1.Result;

                        switch (checkResult.ResultType)
                        {
                            case IdeaRS.OpenModel.Concrete.CheckResult.CheckResultType.Capacity:
                                var resultCapacity = checkResult1 as IdeaRS.OpenModel.Concrete.CheckResult.ConcreteULSCheckResultDiagramCapacityEc2;
                                var fu1 = resultCapacity.Fu1;
                                var fu2 = resultCapacity.Fu2;
                                break;

                            case IdeaRS.OpenModel.Concrete.CheckResult.CheckResultType.Interaction:
                                var resultInteraction = checkResult1 as IdeaRS.OpenModel.Concrete.CheckResult.ConcreteULSCheckResultInteractionEc2;
                                var checkVT = resultInteraction.CheckValueShearAndTorsion;
                                var checkVTB = resultInteraction.CheckValueShearTorsionAndBending;
                                break;
                        }

                        if (checkResult1.NonConformities.Count > 0)
                        {
                            var issues = rcsController.GetNonConformityIssues(checkResult1.NonConformities.Select(it => it.Guid).ToList());
                            foreach (var issue in issues)
                            {
                                System.Diagnostics.Debug.WriteLine(issue.Description);
                            }
                        }
                    }
                }
            }

            rcsController.Dispose();
        }
    }
}
