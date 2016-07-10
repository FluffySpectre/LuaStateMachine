using LuaInterface;

...


// Init VM and load script
var mLuaVM = new Lua();
mLuaVM.DoFile("state.lua");

// Setup State Machine
var FSM = new ScriptedStateMachine<GameEntity>(this);

// Switch to the test state
FSM.ChangeState((LuaTable)mLuaVM["State_Test"]);


...