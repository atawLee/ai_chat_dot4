import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/chat_models.dart';

class ChatLobbyRepository {
  final String baseUrl;

  ChatLobbyRepository({required this.baseUrl});

  /// Get all chat rooms from API
  /// 엔드포인트: GET /api/ChatLobby/chatrooms
  Future<List<ChatRoom>> getChatRooms() async {
    try {
      final url = Uri.parse('$baseUrl/api/ChatLobby/chatrooms');
      final response = await http.get(
        url,
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        final List<dynamic> jsonList = json.decode(response.body);
        return jsonList.map((json) => ChatRoom.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load chat rooms: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error fetching chat rooms: $e');
    }
  }
}
