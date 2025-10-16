namespace eGhis_WebService_Core.Exceptions
{
	/// <summary>
	/// 복호화 예외 클래스
	/// </summary>
	internal class CryptoException : Exception
	{
		#region ### Constructors ###
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="message">예외 메세지</param>
		public CryptoException( string message ) : base( message )
		{
		}

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="message">예외 메세지</param>
		/// <param name="innerException">내부 예외</param>
		public CryptoException( string message, Exception innerException ) : base( message, innerException )
		{
		}

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="innerException">내부 예외</param>
		public CryptoException( Exception innerException = null ) : base( "암/복호화 중 오류가 발생하였습니다", innerException )
		{
		}
		#endregion
	}
}
