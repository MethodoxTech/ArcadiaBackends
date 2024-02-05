extends PanelContainer

@onready var command_prompt = get_node("VBoxContainer/Input/HBoxContainer/CommandText")
@onready var output_text = get_node("VBoxContainer/Panel/Messages")

const arcadia_server_url = "ws://localhost:9910/Arcadia"
var _client:WebSocketPeer = null

# Framework Functions
func _ready():
	set_process(false)

func _process(_delta):
	_client.poll()
	var state = _client.get_ready_state()
	if state == WebSocketPeer.STATE_OPEN:
		while _client.get_available_packet_count():
			var message = _client.get_packet().get_string_from_utf8()
			print("Packet: ", message)
			_update_reply(message)
	elif state == WebSocketPeer.STATE_CONNECTING:
		print("Connecting...")
	elif state == WebSocketPeer.STATE_CLOSING:
		# Keep polling to achieve proper close.
		pass
	elif state == WebSocketPeer.STATE_CLOSED:
		var code = _client.get_close_code()
		var reason = _client.get_close_reason()
		print("WebSocket closed with code: %d, reason %s. Clean: %s" % [code, reason, code != -1])
		set_process(false) # Stop processing.
		_update_reply("Server disconnected.")

# Routines
func _update_reply(reply):
	output_text.text += reply + "\n"
	# Auto-scroll to bottom
	output_text.scroll_vertical = output_text.get_line_count()
	
func _send_command(command):
	# TODO: Do necessary client-side command handling
	
	# Send result message
	var message = command
	_client.send_text(message)

# Signals
func _on_connect_button_pressed():
	_client = WebSocketPeer.new()
	_client.connect_to_url(arcadia_server_url)
	set_process(true)
	_update_reply("Connecting to Arcadia server...")

func _on_command_text_text_submitted(new_text):
	if !command_prompt.text.is_empty():
		_send_command(command_prompt.text)
		command_prompt.clear()

func _on_send_message_button_pressed():
	if !command_prompt.text.is_empty():
		_send_command(command_prompt.text)
		command_prompt.clear()
