using LuaInterface;

public class LuaStateMachine<T> where T : class {
	private LuaTable mCurrentState = null;
	private LuaTable mGlobalState = null;
	private LuaTable mPreviousState = null;
	
	// Reference to the Owner of the statemachine
	private T mOwner;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="owner">Owner.</param>
	public LuaStateMachine(T owner) {
		mOwner = owner;
	}

	/// <summary>
	/// Changes the current state to a new one
	/// </summary>
	/// <param name="newState">The new state</param>
	public void ChangeState(LuaTable newState) {
		if (mCurrentState != null)
			((LuaFunction)mCurrentState["Exit"]).Call(mOwner);

		mPreviousState = mCurrentState;
		mCurrentState = newState;
		((LuaFunction)mCurrentState["Enter"]).Call(mOwner);
	}

	/// <summary>
	/// Changes the state of the global.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void ChangeGlobalState(LuaTable newState) {
		if (mGlobalState != null)
			((LuaFunction)mGlobalState["Exit"]).Call(mOwner);

		mGlobalState = newState;
		((LuaFunction)mGlobalState["Enter"]).Call(mOwner);
	}

	/// <summary>
	/// Reverts the state of the to previous.
	/// </summary>
	public void RevertToPreviousState() {
		if (mPreviousState != null) 
			ChangeState(mPreviousState);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	public void Update() {
		if (mGlobalState != null)
			((LuaFunction)mGlobalState["Execute"]).Call(mOwner);

		if (mCurrentState != null)
			((LuaFunction)mCurrentState["Execute"]).Call(mOwner);
	}
	
	/// <summary>
	/// Handles the message.
	/// </summary>
	/// <returns><c>true</c>, if message was handled, <c>false</c> otherwise.</returns>
	/// <param name="message">Message.</param>
	public bool HandleMessage(Telegram message) {
		// current state
		if (mCurrentState != null) {
			if ((bool)((LuaFunction)mCurrentState["OnMessage"]).Call(mOwner, message)[0]) {
				return true;
			}
		}
		
		// global state
		if (mGlobalState != null) {
			if ((bool)((LuaFunction)mGlobalState["OnMessage"]).Call(mOwner, message)[0]) {
				return true;
			}
		}
		
		return false;
	}

	/// <summary>
	/// Gets the name of the current state.
	/// </summary>
	/// <returns>The current state name.</returns>
	public string GetCurrentStateName() {
		return mCurrentState.ToString();
	}
}
