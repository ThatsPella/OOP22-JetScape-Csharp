using System.Collections.Generic;
using System.Drawing;
using System.Collections.Immutable;


namespace JetScape.Collisions.Hitbox
{
    public abstract class HitboxInstance : IHitbox
    {
        private const int SPRITE_DIMENSIONS = 32;
        private const int CURRENT_TILE_SIZE = 32;
        private IDictionary<Rectangle, Point> _hitboxes;
        private Point _currentPos;
        private readonly ISet<Rectangle> _rectangles;

        public HitboxInstance(Point startPos)
        {
            _hitboxes = new Dictionary<Rectangle, Point>();
            _currentPos = startPos;
            _rectangles = new HashSet<Rectangle>();
        }

        public void UpdatePosition(Point pos)
        {
            var map = new Dictionary<Rectangle, Point>();
            foreach (Rectangle rect in _hitboxes.Keys)
            {
                map.Add(new Rectangle(pos.X + _hitboxes[rect].X, pos.Y + _hitboxes[rect].Y,
                    rect.Width, rect.Height), _hitboxes[rect]);
            }
            _hitboxes = map;
        }

        public ISet<Rectangle> GetRectangles() => _rectangles.ToImmutableHashSet();

        protected void addRectangle(double x, double y, double width, double height)
        {
            int startingX = (int)(_currentPos.X + scale(x));
            int startingY = (int)(_currentPos.Y + scale(y));
            int scaledWidth = (int)scale(width);
            int scaledHeight = (int)scale(height);
            _hitboxes.Add(new Rectangle(startingX, startingY, scaledWidth, scaledHeight),
                    new Point((int)scale(x), (int)scale(y)));
            _rectangles.UnionWith(_hitboxes.Keys);
        }

        protected void addHitbox(IHitbox hitbox) => _rectangles.UnionWith(hitbox.GetRectangles());

        private double scale(double x) => CURRENT_TILE_SIZE * (x / SPRITE_DIMENSIONS);

    }
}