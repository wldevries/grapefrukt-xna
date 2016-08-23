using System;
using System.Collections.Generic;
using System.Linq;
using FlashAnimations.Import;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlashAnimations
{
    public class AnimationPlayer
    {
        private Animation animation;
        private TimeSpan elapsed;
        private float fps = 24;
        private Dictionary<string, Sprite> textures;
        private Action continuation;
        private Textures sheets;
        
        public AnimationPlayer(Animation animation, Textures sheets, Action continuation)
        {
            this.animation = animation;
            this.sheets = sheets;
            this.continuation = continuation;
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public bool Loop { get; set; }

        public void LoadContent(ContentManager content)
        {
            this.textures = new Dictionary<string, Sprite>();
            foreach(var part in animation.Parts)
            {
                var textureDef = this.sheets.Sheets.SelectMany(s => s.Textures).Single(s => s.name == part.name);
                var extensionIndex = System.IO.Path.GetExtension(textureDef.path);
                var path = textureDef.path.Substring(0, textureDef.path.Length - extensionIndex.Length);
                this.textures[textureDef.name] = new Sprite
                {
                    Texture = content.Load<Texture2D>(path),
                    Definition = textureDef
                };
            }
        }

        public void Update(GameTime gameTime)
        {
            this.elapsed += gameTime.ElapsedGameTime;
            if (this.elapsed.TotalSeconds * fps > this.animation.frameCount)
            {
                this.continuation?.Invoke();
            }
        }

        public void Draw(GameTime gameTime)
        {
            var sb = Game1.spriteBatch;

            float rawIndex = (float)(elapsed.TotalSeconds * fps) % this.animation.frameCount;
            int currentIndex, nextIndex;
            float weight;

            if (this.Loop)
            {
                currentIndex = (int)Math.Floor(rawIndex);
                nextIndex    = (currentIndex + 1) % this.animation.frameCount;
                weight       = rawIndex % 1;
            }
            else
            {
                currentIndex = Math.Min((int)Math.Floor(rawIndex), this.animation.frameCount);
                nextIndex    = Math.Min(currentIndex + 1, this.animation.frameCount);
                weight       = rawIndex % 1;
            }

            sb.Begin();
            var parts = this.textures
                .OrderBy(t => t.Value.Definition.zIndex)
                .Select(s => this.animation.Parts.Single(p => p.name == s.Key));
            foreach(var part in parts)
            {
                DrawPart(sb, part, currentIndex, nextIndex, weight);
            }
            sb.End();
        }

        private void DrawPart(SpriteBatch sb, Part part, int currentIndex, int nextIndex, float weight)
        {
            var sprite = textures[part.name];
            var currentFrame = part.Frames.FirstOrDefault(f => f.index == currentIndex);
            var nextFrame    = part.Frames.FirstOrDefault(f => f.index == nextIndex);
            var pos          = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.Position);
            var scale        = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.Scale);
            var rotation     = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.rotation);
            rotation         = MathHelper.ToRadians(rotation);
            var alpha        = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.alpha);
            // All origins are centered on the textures
            var origin = new Vector2(
                sprite.Definition.registrationPointX,
                sprite.Definition.registrationPointY);
            if (sprite.Definition.FrameCount <= 1)
            {
                sb.Draw(sprite.Texture,
                    this.Position + this.Scale * pos,
                    null,
                    Color.White * alpha,
                    rotation,
                    origin,
                    this.Scale * scale,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                int animationFrame = (int)(elapsed.TotalSeconds * fps) % sprite.Definition.FrameCount;

                int col = animationFrame % sprite.Definition.Columns;
                int row = animationFrame / sprite.Definition.Columns;
                int x = col * sprite.Definition.frameWidth;
                int y = row * sprite.Definition.frameWidth;
                var sourcerect = new Rectangle(x, y, sprite.Definition.frameWidth, sprite.Definition.frameHeight);

                sb.Draw(sprite.Texture,
                    this.Position + this.Scale * pos,
                    sourcerect,
                    Color.White * alpha,
                    rotation,
                    origin,
                    this.Scale * scale,
                    SpriteEffects.None,
                    0);
            }
        }

        public class Sprite
        {
            public Texture2D Texture;
            public TextureDef Definition;
        }
    }
}
