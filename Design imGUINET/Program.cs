using ImGuiNET;
using ClickableTransparentOverlay;
using System.Numerics;
using System.Runtime.CompilerServices;
using Design_imGUINET;
using System.Drawing;
using Color = System.Drawing.Color;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UI{ 
    public class Program : Overlay
    {
        ImGuiIOPtr io;
        ImGuiStylePtr style;
        bool init = false;
        public static float globalOpacity = 1.0f;
        public static Program _Program;
        List<Product> products = new List<Product>();
        protected void initialize()
        {
            init = true;
            ImGuiTheme.initDefaultTheme();
            style.WindowRounding = 6;
            var dictionary = new Dictionary<string, Tuple<string, float, FontGlyphRangeType, ushort[]>>();
            dictionary.Add("Poppins-Medium-20", new Tuple<string, float, FontGlyphRangeType, ushort[]>(@"Fonts\Poppins-Medium.ttf", 20, FontGlyphRangeType.English, null));
            dictionary.Add("Poppins-Bold-32", new Tuple<string, float, FontGlyphRangeType, ushort[]>(@"Fonts\Poppins-Bold.ttf", 42, FontGlyphRangeType.English, null));
            dictionary.Add("Poppins-Medium-24", new Tuple<string, float, FontGlyphRangeType, ushort[]>(@"Fonts\Poppins-Medium.ttf", 24, FontGlyphRangeType.English, null));

            dictionary.Add("icons", new Tuple<string, float, FontGlyphRangeType, ushort[]>(@"Fonts\fa-solid-900.ttf", 18, FontGlyphRangeType.English, new ushort[] { 0xe005,
            0xf8ff,0}));
            globalOpacity = 0;
            tranisitioning = true;
            tranisitionID = transitionToUI.login;
            this.InitializeFonts(dictionary);
            IntPtr ImageHandle;
            uint W, H;
            this.AddOrGetImagePointer(@"Images\EFT.jpg", false,out ImageHandle,out W,out H);
            products.Add(new Product()
            {
                image = ImageHandle,
                Available = true,
                date = new DateTime(2023, 1, 1),
                name = "Escape From Tarkov"
            });
            this.AddOrGetImagePointer(@"Images\Rust.jpg", false, out ImageHandle, out W, out H);
            products.Add(new Product()
            {
                image = ImageHandle,
                Available = false,
                date = new DateTime(2023, 1, 11),
                name = "Rust"
            });
            this.AddOrGetImagePointer(@"Images\Fortnite.jpg", false, out ImageHandle, out W, out H);
            products.Add(new Product()
            {
                image = ImageHandle,
                Available = false,
                date = new DateTime(2022, 11, 21),
                name = "Fortnite"
            });
            this.AddOrGetImagePointer(@"Images\Apex.jpg", false, out ImageHandle, out W, out H);
            products.Add(new Product()
            {
                image = ImageHandle,
                Available = false,
                date = new DateTime(2022, 12, 21),
                name = "Apex"
            });
            this.AddOrGetImagePointer(@"Images\Valorant.jpg", false, out ImageHandle, out W, out H);
            products.Add(new Product()
            {
                image = ImageHandle,
                Available = false,
                date = new DateTime(2022, 1, 22),
                name = "Valorant"
            });
        }
        public ImGuiTheme.glowingInputConfigurator getTextTheme()
        {
            ImGuiTheme.glowingInputConfigurator textboxTheme = new ImGuiTheme.glowingInputConfigurator();
            textboxTheme.size = new Vector2(414, 50);
            textboxTheme.roundCorners = 5;
            textboxTheme.prefix = "Username";
            textboxTheme.borderThickness = 3f;
            textboxTheme.bgcolor = Color.FromArgb(28, 28, 32).ToUint();
            textboxTheme.borderColor = Color.FromArgb(88, 37, 227).ToUint();
            textboxTheme.borderColorActive = Color.FromArgb(115, 70, 232).ToUint();
            textboxTheme.textColor = Color.White.ToUint();
            textboxTheme.fontScale = 16f;
            return textboxTheme;
        }
        public ImGuiTheme.SliderInputConfigurator getSliderTheme()
        {
            ImGuiTheme.SliderInputConfigurator textboxTheme = new ImGuiTheme.SliderInputConfigurator();
            textboxTheme.size = new Vector2(400, 10);
            textboxTheme.roundCorners = 5;
            textboxTheme.borderThickness = 3f;
            textboxTheme.bgcolor = Color.FromArgb(28, 28, 32).ToUint();
            textboxTheme.borderColor = Color.FromArgb(88, 37, 227).ToUint();
            textboxTheme.borderColorActive = Color.FromArgb(115, 70, 232).ToUint();
            textboxTheme.sliderColor = Color.FromArgb(82, 34, 204).ToUint();
            textboxTheme.sliderColorActive = Color.FromArgb(119, 73, 226).ToUint();
            textboxTheme.YoffsetLabel = -20;
            return textboxTheme;
        }
        public ImGuiTheme.ButtonConfigurator getButtonTheme()
        {
            ImGuiTheme.ButtonConfigurator textboxTheme = new ImGuiTheme.ButtonConfigurator();
            textboxTheme.size = new Vector2(214, 50);
            textboxTheme.roundCorners = 5;
            textboxTheme.text = "NULL";
            textboxTheme.bgcolor = Color.FromArgb(91, 36, 221).ToUint();
            textboxTheme.ColorHover = Color.FromArgb(114, 71, 224).ToUint();
            textboxTheme.textColor = Color.White.ToUint();
            textboxTheme.waitSpeed = 0.00025f;
            textboxTheme.SlideSpeed = 0.0025f;
            textboxTheme.circleThickness = 4;
            textboxTheme.circleRadius = 20;
            textboxTheme.circleColor = Color.FromArgb(82, 34, 204).ToUint();
            textboxTheme.circleSpeed = 0.005f;
            textboxTheme.circlePositionY = -90;
            return textboxTheme;
        }
        public Program() : base("Unknown", true)
        {
            base.VSync = false;
        }
        string username = "", password = "", Email = "";
        bool tranisitioning = false;
        float transitionValue = 0;
        transitionToUI tranisitionID;
        enum transitionToUI
        {
            login = 0,
            Error = 1,
            signUp = 2,
            Menu = 3,
        }
        protected void TransitionToSignUp()
        {
            if(transitionValue >= 1)
            {
                tranisitioning = false;
                transitionValue = 0;
                return;
            }
            transitionValue += 0.0005f;
            if (transitionValue >= 0.5f)
            {
                uiIdx = (int)tranisitionID;
                globalOpacity += 0.001f;
            }
            else globalOpacity -= 0.001f;
            if (globalOpacity > 1) globalOpacity = 1;
            if (globalOpacity < 0) globalOpacity = 0;

        }
        protected void renderSignUp()
        {
            Vector2 windowPos = ImGui.GetWindowPos();
            ImGui.SetNextWindowSize(new Vector2(535, 570));
            ImGui.Begin("c# UI", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            ImGuiTheme.drawExitButton(15, Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PushFont(getFontByLabel("icons"));
            var startDrawingIcons = new Vector2(30, 10);
            ImGuiTheme.drawTextGradient("searchButton", startDrawingIcons, FontAwesome5.Search,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Search).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("storeButton", startDrawingIcons, FontAwesome5.Store,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Store).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("UniversityButton", startDrawingIcons, FontAwesome5.University,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PopFont();
            ImGuiTheme.gradientBorder(Color.FromArgb(88, 37, 227).toVec4(), style.Colors[ImGuiCol.WindowBg.toInt()]);
            ImGuiTheme.glowingInputConfigurator textboxTheme = getTextTheme();
            ImGuiTheme.ButtonConfigurator buttonTheme = getButtonTheme();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.PushFont(this.getFontByLabel("Poppins-Bold-32"));
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - ImGui.CalcTextSize("SIGN UP").X / 2, 0);
            ImGui.TextColored(Color.FromArgb((int)(255* globalOpacity),88, 37, 227).toVec4(), "SIGN UP");
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.PopFont();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            ImGuiTheme.glowingInput("test2", ref username, textboxTheme);
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            textboxTheme.prefix = "Email";
            ImGuiTheme.glowingInput("test455", ref Email, textboxTheme);
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            textboxTheme.passwordChar = '*';
            textboxTheme.prefix = "Password";
            ImGuiTheme.glowingInput("test45", ref password, textboxTheme);
            ImGui.NewLine();
            ImGui.PushFont(getFontByLabel("icons"));
            var startDrawingUser = new Vector2(ImGui.GetContentRegionAvail().X / 2 + 15 + ImGui.CalcTextSize(FontAwesome5.UserCheck).X / 2, ImGui.GetCursorPosY() + globalOpacity * 60);
            
           if(ImGuiTheme.drawTextGradient("userCheck", startDrawingUser, FontAwesome5.UserCheck,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227)))
            {

                tranisitionID = transitionToUI.login;
                tranisitioning = true;
            }


            var startDrawingUser2 = new Vector2(ImGui.GetContentRegionAvail().X / 2 - 15 - ImGui.CalcTextSize(FontAwesome5.UserPlus).X / 2, ImGui.GetCursorPosY() + globalOpacity * 60);
            ImGuiTheme.drawTextGradient("userPlus", startDrawingUser2, FontAwesome5.UserPlus,
                 Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            

            ImGui.PopFont();


            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            buttonTheme.text = "Launch";
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - buttonTheme.size.X / 2, 0);
            if (ImGuiTheme.buttonWait("test22", buttonTheme))
            {
                uiIdx = 1;
            }
            ImGui.End();
        }
        struct Product
        {
            public IntPtr image;
            public DateTime date;
            public string name;
            public bool Available;
        }
        int toProduct = 0;
        float transitionProductprogress = 0;
        float productOpacity = 1.0f;
        bool transitioningProduct = false;
        protected void transitionProduct()
        {
            if (transitionProductprogress >= 0.5) {
                selectedProductIdx = toProduct;
                productOpacity += 0.001f;
            } else
                productOpacity -= 0.001f;
            transitionProductprogress += 0.0005f;
            if (productOpacity > 1) productOpacity = 1;
            if (productOpacity < 0) productOpacity = 0;
            if (transitionProductprogress >= 1)
            {
                transitioningProduct = false;
                transitionProductprogress = 0;
            }
        }
        int selectedProductIdx;
        protected void renderMenu()
        {

            if (transitioningProduct)
                transitionProduct();

            ImGui.SetNextWindowSize(new Vector2(535, 570));
            ImGui.Begin("c# UI", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            Vector2 windowPos = ImGui.GetWindowPos();
            Vector2 windowSize = ImGui.GetContentRegionAvail();
            ImGuiTheme.drawExitButton(15, Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PushFont(getFontByLabel("icons"));
            var startDrawingIcons = new Vector2(30, 10);
            if(
            ImGuiTheme.drawTextGradient("AllignLeft",
    new Vector2(windowSize.X / 2 - ImGui.CalcTextSize(FontAwesome5.ArrowLeft).X / 2 - 40, 380), FontAwesome5.ArrowLeft,
    Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227)))
            {
                toProduct = selectedProductIdx-1;
                transitioningProduct = true;
            }
            if (ImGuiTheme.drawTextGradient("ArrowRight",
            new Vector2(windowSize.X / 2 - ImGui.CalcTextSize(FontAwesome5.ArrowRight).X / 2 + 40, 380), FontAwesome5.ArrowRight,
    Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227)))
            {
                toProduct = selectedProductIdx+1;
                transitioningProduct = true;
            }
            if (toProduct < 0) toProduct = 0;
            if (toProduct >= products.Count) toProduct = products.Count - 1;
            ImGuiTheme.drawTextGradient("searchButton", startDrawingIcons, FontAwesome5.Search,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Search).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("storeButton", startDrawingIcons, FontAwesome5.Store,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Store).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("UniversityButton", startDrawingIcons, FontAwesome5.University,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PopFont();
            ImGuiTheme.gradientBorder(Color.FromArgb(205, 65, 111).toVec4(), style.Colors[ImGuiCol.WindowBg.toInt()]);
            ImGuiTheme.glowingInputConfigurator textboxTheme = getTextTheme();
            ImGuiTheme.ButtonConfigurator buttonTheme = getButtonTheme();
            Product selectedProduct = products[selectedProductIdx];
            var draw = ImGui.GetWindowDrawList();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            Vector2 startDrawImg = new Vector2(windowPos.X + ImGui.GetWindowSize().X / 2 - 205, windowPos.Y + ImGui.GetCursorPosY());
            Vector2 EndDrawImg = new Vector2(windowPos.X + ImGui.GetWindowSize().X / 2 + 205, windowPos.Y + ImGui.GetCursorPosY() + 230);
            Vector2 imageSize = new Vector2(410, 230);
            draw.AddImage(selectedProduct.image, startDrawImg,EndDrawImg, new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),Color.FromArgb((int)(productOpacity*255.0f),255,255,255).ToUint());
            draw.AddRect(startDrawImg, EndDrawImg, Color.FromArgb((int)(productOpacity * 255.0f), 215, 104, 140).ToUint(), 5);
            draw.AddRectFilled(new Vector2(startDrawImg.X + imageSize.X / 2.0f - 160, startDrawImg.Y + 1)
                , new Vector2(startDrawImg.X + imageSize.X / 2.0f + 160, startDrawImg.Y + 41), Color.FromArgb((int)(150.0f* productOpacity), 0,0,0).ToUint(), 5,ImDrawFlags.RoundCornersBottom);
            draw.AddRectFilled(new Vector2(startDrawImg.X + 1, startDrawImg.Y + imageSize.Y - 41),
                new Vector2(startDrawImg.X + 1 + imageSize.X / 2, startDrawImg.Y + imageSize.Y -1),
                Color.FromArgb((int)(150.0f * productOpacity), 0, 0, 0).ToUint(), 5, ImDrawFlags.RoundCornersTop);
            draw.AddText(new Vector2(startDrawImg.X + imageSize.X / 2 - ImGui.CalcTextSize(selectedProduct.name).X / 2,
                startDrawImg.Y + 20.5f - ImGui.CalcTextSize(selectedProduct.name).Y / 2)
                , Color.White.Brightness(productOpacity).ToUint(), selectedProduct.name);

            draw.AddRectFilled(new Vector2(EndDrawImg.X - 41, startDrawImg.Y + imageSize.Y - 41),
                new Vector2(EndDrawImg.X - 1, EndDrawImg.Y-1),
                Color.FromArgb((int)(150.0f* productOpacity), 0, 0, 0).ToUint(), 5, ImDrawFlags.RoundCornersTop);
            draw.AddCircle(
               new Vector2(EndDrawImg.X - 20.5f,EndDrawImg.Y - 20.5f), 5,
               (selectedProduct.Available) ? Color.FromArgb((int)(255.0f* productOpacity),255, 0,0).ToUint() : Color.FromArgb((int)(255.0f * productOpacity), 0, 255, 0).ToUint());

            draw.AddText(new Vector2(startDrawImg.X + 1 + (imageSize.X / 2) / 2 - ImGui.CalcTextSize(selectedProduct.date.ToString("dd/MM/yyyy")).X / 2
                , startDrawImg.Y + imageSize.Y - (41 /2) - ImGui.CalcTextSize(selectedProduct.date.ToString("dd/MM/yyyy")).Y / 2), Color.White.Brightness(productOpacity).ToUint(), selectedProduct.date.ToShortDateString());
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine(); 
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            buttonTheme.text = "Inject";
            buttonTheme.circleColor = Color.FromArgb(205, 65, 111).ToUint();
            buttonTheme.bgcolor = Color.FromArgb(205, 65, 111).ToUint();
            buttonTheme.ColorHover = Color.FromArgb(215, 104, 140).ToUint();
            ImGui.SameLine(windowSize.X / 2 - buttonTheme.size.X / 2, 0);
            if (ImGuiTheme.buttonWait("test22", buttonTheme))
            {
                uiIdx = 1;
            }
            ImGui.End();
        }
        protected void renderLogin()
        {
            Vector2 windowPos = ImGui.GetWindowPos();
            ImGui.SetNextWindowSize(new Vector2(535, 570));
            ImGui.Begin("c# UI", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            ImGuiTheme.drawExitButton(15, Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PushFont(getFontByLabel("icons"));
            var startDrawingIcons = new Vector2(30, 10);
            ImGuiTheme.drawTextGradient("searchButton", startDrawingIcons, FontAwesome5.Search,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Search).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("storeButton", startDrawingIcons, FontAwesome5.Store,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            startDrawingIcons = new Vector2(startDrawingIcons.X + ImGui.CalcTextSize(FontAwesome5.Store).X + 30, startDrawingIcons.Y);
            ImGuiTheme.drawTextGradient("UniversityButton", startDrawingIcons, FontAwesome5.University,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));
            ImGui.PopFont();
            ImGuiTheme.gradientBorder(Color.FromArgb(88, 37, 227).toVec4(), style.Colors[ImGuiCol.WindowBg.toInt()]);
            ImGuiTheme.glowingInputConfigurator textboxTheme = getTextTheme();
            ImGuiTheme.ButtonConfigurator buttonTheme = getButtonTheme();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.PushFont(this.getFontByLabel("Poppins-Bold-32"));
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - ImGui.CalcTextSize("SIGN IN").X / 2, 0);
            ImGui.TextColored(Color.FromArgb((int)(255 * globalOpacity), 88, 37, 227).toVec4(), "SIGN IN");
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.PopFont();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            ImGuiTheme.glowingInput("test2", ref username, textboxTheme);
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            textboxTheme.passwordChar = '*';
            textboxTheme.prefix = "Password";
            ImGuiTheme.glowingInput("test45", ref password, textboxTheme);
            ImGui.NewLine();
            ImGui.PushFont(getFontByLabel("icons"));
            var startDrawingUser = new Vector2(ImGui.GetContentRegionAvail().X / 2 + 15 + ImGui.CalcTextSize(FontAwesome5.UserCheck).X / 2 ,ImGui.GetCursorPosY() + (1 - globalOpacity) * 60);
            ImGuiTheme.drawTextGradient("userCheck", startDrawingUser, FontAwesome5.UserCheck,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227));


            var startDrawingUser2 = new Vector2(ImGui.GetContentRegionAvail().X / 2 -15 - ImGui.CalcTextSize(FontAwesome5.UserPlus).X / 2, ImGui.GetCursorPosY() + (1 - globalOpacity) * 60);
           if(ImGuiTheme.drawTextGradient("userPlus", startDrawingUser2, FontAwesome5.UserPlus,
                Color.FromArgb(54, 52, 58), Color.FromArgb(88, 37, 227)))
            {
                tranisitionID = transitionToUI.signUp;
                tranisitioning = true;
            }

            ImGui.PopFont();


            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - textboxTheme.size.X / 2, 0);
            buttonTheme.text = "Launch";
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.SameLine(ImGui.GetContentRegionAvail().X / 2 - buttonTheme.size.X / 2, 0);
           if(ImGuiTheme.buttonWait("test22", buttonTheme))
            {
                tranisitionID = transitionToUI.Menu;
                tranisitioning = true;
            }
            ImGui.End();
        }
        float value = 0.0f;
        protected void renderDown()
        {
            ImGui.SetNextWindowSize(new Vector2(535, 570));
            ImGui.Begin("c# UI", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            Vector2 windowPos = ImGui.GetWindowPos();
            Vector2 windowSize = ImGui.GetWindowSize();
            var draw = ImGui.GetWindowDrawList();
            ImGuiTheme.gradientBorder(Color.FromArgb(88, 37, 227).toVec4(), style.Colors[ImGuiCol.WindowBg.toInt()]);
            ImGuiTheme.circleProgressBarAnimated("temporarily down circle",
                new Vector2(windowSize.X / 2,windowSize.Y / 2 - 20),20,Color.FromArgb(86, 36, 219).ToUint(),3,0.001f,.95f);
            ImGui.PushFont(getFontByLabel("Poppins-Medium-24"));
            draw.AddText(new Vector2(windowPos.X + windowSize.X / 2 - ImGui.CalcTextSize("Loader is temporarily down \r\n try again later").X / 2, windowPos.Y + windowSize.Y / 2 + 10)
                , Color.FromArgb(86, 36, 219).ToUint(), "Loader is temporarily down \r\n try again later");
            ImGui.PopFont();
            ImGui.End();
        }
        int uiIdx = 0;
        protected override void Render()
        {
            ImGuiTheme.AnimateProperties();
            io = ImGui.GetIO();
            style = ImGui.GetStyle();
            if (!init)
            {
                initialize();
            }
            if (tranisitioning)
                TransitionToSignUp();
            switch (uiIdx)
            {
                case 0:
                    renderLogin();
                    break;
                case 1:
                    renderDown();
                    break;
                case 2:
                    renderSignUp();
                    break;
                case 3:
                    renderMenu();
                    break;
            }
        }
        public static void Main(string[] args)
        {
            Program._Program = new Program();
            Program._Program.Start().Wait();
        }
    }


}