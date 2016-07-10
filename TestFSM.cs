using LuaInterface;

...


// Init VM and load script
var luaVM = new Lua();
luaVM.DoFile("state.lua");

// Setup State Machine
var FSM = new ScriptedStateMachine<GameEntity>(this);

// Switch to the test state
FSM.ChangeState((LuaTable)luaVM["State_Test"]);


...