#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <conio.h>

char convert_num_to_char(int num) {
  if ((int)(num / 10) == 0) {
    switch (num) {
      case 1:
        return '1';
        break;
      case 2:
        return '2';
        break;
      case 3:
        return '3';
        break;
      case 4:
        return '4';
        break;
      case 5:
        return '5';
        break;
      case 6:
        return '6';
        break;
      case 7:
        return '7';
        break;
      case 8:
        return '8';
        break;
      case 9:
        return '9';
        break;
      case 0:
        return '0';
        break;
      default:
        break;
    }
  } else {
    printf("Invalid value passed. (%d)", num);
    exit(-1);
  }
}

char* insert_char(char* str, char c, int index) {
  int length = strlen(str) + 2;
  char* new_str = malloc(length * sizeof(char));

  int j = 0;
  for (int i = 0; i < strlen(str); i++) {
    if (i == index) {
      new_str[j] = c;
      j++;
    }
    new_str[j] = str[i];
    j++;
  }

  if (index == strlen(str)) {
    new_str[strlen(str)] = c;
    j++;
  }

  new_str[j] = '\0';
  return new_str;
}

char* insert_string(char* str, char* str_to_be_added, char indicator) {
  
  for (int i = 0; i < strlen(str); i++) {
    if (str[i] == indicator) {
      for (int j = strlen(str_to_be_added) - 1; j >= 0; j--) {
        str = insert_char(str, str_to_be_added[j], i + 1);
      }
    }
  }
  return str;
}

void print_screen(int cols, int rows, char content[rows][cols], int saved) {

  char* final_string = "=-=-=-=-=-(Saved: _ | File Name: text.txt | Size: 0kb)-=-=-=-=-=\n";

  final_string = insert_string(final_string, (saved == 1) ? "TRUE" : "FALSE", '_');
  
  for (int i = 0; i < rows; i++) {
    final_string = insert_char(final_string, ' ', strlen(final_string));
    final_string = insert_char(final_string, convert_num_to_char(i + 1), strlen(final_string));
    final_string = insert_char(final_string, ':', strlen(final_string));
    final_string = insert_char(final_string, ' ', strlen(final_string));
    for (int j = 0; j < cols; j++) {
      final_string = insert_char(final_string, content[i][j], strlen(final_string));
    }
    final_string = insert_char(final_string, '\n', strlen(final_string));
  }
  printf(final_string);
  // free(final_string);
}

int main() {

  char content[5][5];
  for (int i = 0; i < 5; i++) {
    for (int j = 0; j < 5; j++) {
      content[i][j] = '0';
    }
  }

  int saved = 0;

  int cursor_x = 0;
  int cursor_y = 0;

  system("cls");

  while (1) {
    switch (getch()) {
      case 224:
        switch (getch()) {
          case 72:
            cursor_y -= 1;
            break;
          case 80:
            cursor_y += 1;
            break;
          case 65:
            cursor_x -= 1;
            break;
          case 77:
            cursor_x += 1;
            break;
        }
        break;
      case 'c': 
        exit(0);
        break;
      default:
        break;
    }

    print_screen(5, 5, content, saved);
    
    //printf("%d - ", cursor_x);
    //printf("%d\n", cursor_y);
  }
  return 0;
}