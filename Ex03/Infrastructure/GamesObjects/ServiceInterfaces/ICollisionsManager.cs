//*** Guy Ronen (c) 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ServiceInterfaces
{   
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;
        event EventHandler<EventArgs> SizeChanged;
        event EventHandler<EventArgs> VisibleChanged;
        event EventHandler<EventArgs> Disposed;
        bool Visible { get; }
        bool CheckCollision(ICollidable i_Source);
        void Collided(ICollidable i_Collidable);
    }
   
    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        Vector2 Velocity { get; }
    }
  
    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }
        Vector3 Velocity { get; }
    }
   
    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
    
}
