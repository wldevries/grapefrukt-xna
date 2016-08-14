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
        private Dictionary<string, Texture2D> textures;
        private Action continuation;

        public AnimationPlayer(Animation animation, Action continuation)
        {
            this.animation = animation;
            this.continuation = continuation;
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public bool Loop { get; set; }
        public string[] DrawOrder { get; set; }

        public void LoadContent(ContentManager content)
        {
            this.textures = new Dictionary<string, Texture2D>();
            foreach(var part in animation.Parts)
            {
                this.textures[part.name] = content.Load<Texture2D>(part.name);
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
            var parts = this.DrawOrder.Select(name => this.animation.Parts.Single(p => p.name == name));
            foreach(var part in parts)
            {
                DrawPart(sb, part, currentIndex, nextIndex, weight);
            }
            sb.End();
        }

        private void DrawPart(SpriteBatch sb, Part part, int currentIndex, int nextIndex, float weight)
        {
            var currentFrame = part.Frames.FirstOrDefault(f => f.index == currentIndex);
            var nextFrame    = part.Frames.FirstOrDefault(f => f.index == nextIndex);
            var pos          = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.Position);
            var scale        = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.Scale);
            var rotation     = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.rotation);
            rotation         = MathHelper.ToRadians(rotation);
            var alpha        = Frame.LerpValue(currentFrame, nextFrame, weight, f => f.alpha);
            // All origins are centered on the textures
            var origin       = new Vector2(textures[part.name].Width, textures[part.name].Height) / 2;
            sb.Draw(textures[part.name],
                this.Position + pos,
                null,
                Color.White * alpha,
                rotation,
                origin,
                this.Scale * scale,
                SpriteEffects.None, 0);
        }
    }
}
