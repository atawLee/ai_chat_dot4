import 'package:flutter/foundation.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ApiSetting extends ChangeNotifier {
  static final ApiSetting _instance = ApiSetting._internal();
  factory ApiSetting() => _instance;
  ApiSetting._internal();

  // 싱글턴 인스턴스에 직접 접근할 수 있는 static getter
  static ApiSetting get instance => _instance;

  String _defaultAddress = '';

  // Getter
  String get defaultAddress => _defaultAddress;

  // Setter
  set defaultAddress(String address) {
    if (_defaultAddress != address) {
      _defaultAddress = address;
      notifyListeners(); // 변경 알림
    }
  }
}

final apiProvider = ChangeNotifierProvider<ApiSetting>((ref) {
  return ApiSetting.instance; // static getter로 싱글턴 인스턴스 참조
});

// 사용자 닉네임 Provider
final userNicknameProvider = FutureProvider<String?>((ref) async {
  SharedPreferences prefs = await SharedPreferences.getInstance();
  return prefs.getString('nickname');
});
