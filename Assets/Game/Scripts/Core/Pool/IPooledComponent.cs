namespace AG.Core.Pool
{
		public interface IPooledComponent
	{
		void OnBeforeGetFromPool();
		void OnAfterGetFromPool();
		void OnReturnToPool();
		void OnDestroyFromPool();
	}
}