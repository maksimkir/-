# lab_gui_tf.py
# GUI-додаток для лабораторної роботи:
# - створює TF_1 (генерує приклад) або відкриває наявний TF_1
# - читає TF_1, знаходить слова, що починаються голосною (укр/англ)
# - записує кожне таке слово в окремий рядок TF_2
# - читає TF_2 і показує вміст по рядках у вікні
#
# Потрібно: Python 3 (рекомендую 3.7+). Бібліотека Tkinter вже входить у стандартну поставку.

import tkinter as tk
from tkinter import filedialog, messagebox, scrolledtext
import os
import re

# Набір голосних літер (українські + англійські; враховано верхній/нижній регістр)
VOWELS = set("аеєиіїоуюяАЕЄИІЇОУЮЯaeiouAEIOU")

# Регулярний вираз для "слов": дозволяємо літери (українські/латинські), апостроф та дефіс
WORD_RE = re.compile(r"[A-Za-zА-Яа-яА-ЯЁёЇїЄєІі'-]+", re.UNICODE)

DEFAULT_TF1 = "TF_1.txt"
DEFAULT_TF2 = "TF_2.txt"

class App:
    def __init__(self, root):
        self.root = root
        root.title("Лабораторна: TF_1 → TF_2 (слова, що починаються на голосну)")
        root.geometry("760x520")

        # Верхня панель кнопок
        frm = tk.Frame(root)
        frm.pack(fill="x", padx=8, pady=6)

        btn_gen = tk.Button(frm, text="Згенерувати приклад TF_1", command=self.generate_tf1)
        btn_gen.grid(row=0, column=0, padx=4)

        btn_open = tk.Button(frm, text="Відкрити TF_1 із файлу...", command=self.open_tf1_dialog)
        btn_open.grid(row=0, column=1, padx=4)

        btn_process = tk.Button(frm, text="Обробити TF_1 → TF_2", command=self.process_files)
        btn_process.grid(row=0, column=2, padx=4)

        btn_choose_out = tk.Button(frm, text="Зберегти TF_2 як...", command=self.choose_tf2_dialog)
        btn_choose_out.grid(row=0, column=3, padx=4)

        btn_show = tk.Button(frm, text="Показати TF_2", command=self.show_tf2)
        btn_show.grid(row=0, column=4, padx=4)

        btn_clear = tk.Button(frm, text="Очистити вивід", command=self.clear_output)
        btn_clear.grid(row=0, column=5, padx=4)

        # Поля з інформацією про файли
        files_fr = tk.Frame(root)
        files_fr.pack(fill="x", padx=8)

        tk.Label(files_fr, text="TF_1:").grid(row=0, column=0, sticky="w")
        self.tf1_var = tk.StringVar(value=os.path.abspath(DEFAULT_TF1))
        self.tf1_entry = tk.Entry(files_fr, textvariable=self.tf1_var, width=70)
        self.tf1_entry.grid(row=0, column=1, padx=4, pady=4, sticky="w")

        tk.Label(files_fr, text="TF_2:").grid(row=1, column=0, sticky="w")
        self.tf2_var = tk.StringVar(value=os.path.abspath(DEFAULT_TF2))
        self.tf2_entry = tk.Entry(files_fr, textvariable=self.tf2_var, width=70)
        self.tf2_entry.grid(row=1, column=1, padx=4, pady=4, sticky="w")

        # Текстове поле для виводу
        tk.Label(root, text="Вивід (лог + вміст TF_2):").pack(anchor="w", padx=8)
        self.txt = scrolledtext.ScrolledText(root, wrap="word", height=22)
        self.txt.pack(fill="both", expand=True, padx=8, pady=6)

        # Ініціалізація: якщо TF_1 існує, показати його шлях
        if not os.path.exists(self.tf1_var.get()):
            self.tf1_var.set(os.path.abspath(DEFAULT_TF1))
        if not os.path.exists(self.tf2_var.get()):
            self.tf2_var.set(os.path.abspath(DEFAULT_TF2))

    def log(self, msg=""):
        self.txt.insert("end", msg + "\n")
        self.txt.see("end")

    def clear_output(self):
        self.txt.delete("1.0", "end")

    def generate_tf1(self):
        """Генерує приклад TF_1 з символьних рядків різної довжини"""
        sample_lines = [
            "Олексій прийшов вранці, привітався та сказав: \"Добрий день!\"",
            "apple orange banana; kiwi. Umbrella? dog-cat",
            "Їхали ми, сміялися — були щасливі.",
            "Інженер-фахівець аналізує дані, а оператор працює.",
            "Якщо є питання, пишіть на пошту: test@example.com"
        ]
        path = self.tf1_var.get()
        try:
            with open(path, "w", encoding="utf-8") as f:
                for ln in sample_lines:
                    f.write(ln + "\n")
            self.log(f"Згенеровано TF_1: {path}")
            messagebox.showinfo("Генерація TF_1", f"Файл TF_1 з прикладом збережено у:\n{path}")
        except Exception as e:
            messagebox.showerror("Помилка", f"Не вдалося записати TF_1:\n{e}")
            self.log(f"Помилка запису TF_1: {e}")

    def open_tf1_dialog(self):
        p = filedialog.askopenfilename(title="Виберіть TF_1 (вхідний файл)",
                                       filetypes=[("Text files", "*.txt"), ("All files", "*.*")])
        if p:
            self.tf1_var.set(p)
            self.log(f"Вибрано TF_1: {p}")

    def choose_tf2_dialog(self):
        p = filedialog.asksaveasfilename(title="Зберегти TF_2 як...", defaultextension=".txt",
                                         filetypes=[("Text files", "*.txt"), ("All files", "*.*")])
        if p:
            self.tf2_var.set(p)
            self.log(f"Вибрано TF_2 для збереження: {p}")

    def extract_words_starting_with_vowel(self, text):
        """Повертає список слів, які починаються на голосну (укр/англ)."""
        words = WORD_RE.findall(text)
        result = []
        for w in words:
            if len(w) == 0:
                continue
            first_char = w[0]
            if first_char in VOWELS:
                result.append(w)
        return result

    def process_files(self):
        tf1 = self.tf1_var.get()
        tf2 = self.tf2_var.get()

        if not os.path.exists(tf1):
            messagebox.showwarning("TF_1 не знайдено", f"Файл TF_1 не знайдено:\n{tf1}\nМожете згенерувати приклад або відкрити інший файл.")
            self.log(f"TF_1 не знайдено: {tf1}")
            return

        found_words = []
        try:
            with open(tf1, "r", encoding="utf-8") as f:
                for line_no, line in enumerate(f, start=1):
                    # знаходимо слова в рядку
                    words = self.extract_words_starting_with_vowel(line)
                    if words:
                        self.log(f"Рядок {line_no}: знайдено {len(words)} слов(а) -> {words}")
                    found_words.extend(words)
        except Exception as e:
            messagebox.showerror("Помилка читання TF_1", str(e))
            self.log(f"Помилка читання TF_1: {e}")
            return

        # Записуємо кожне слово в окремий рядок TF_2
        try:
            with open(tf2, "w", encoding="utf-8") as f:
                for w in found_words:
                    f.write(w + "\n")
            self.log(f"Записано {len(found_words)} слов(а) у TF_2: {tf2}")
            messagebox.showinfo("Обробка завершена", f"Знайдено {len(found_words)} слов(а). Результат записано у:\n{tf2}")
        except Exception as e:
            messagebox.showerror("Помилка запису TF_2", str(e))
            self.log(f"Помилка запису TF_2: {e}")
            return

        # Показати вміст TF_2 автоматично
        self.show_tf2()

    def show_tf2(self):
        tf2 = self.tf2_var.get()
        if not os.path.exists(tf2):
            messagebox.showwarning("TF_2 не знайдено", f"Файл TF_2 не знайдено:\n{tf2}\nСпочатку обробіть TF_1.")
            self.log(f"TF_2 не знайдено: {tf2}")
            return
        try:
            self.log("----- Вміст TF_2 -----")
            with open(tf2, "r", encoding="utf-8") as f:
                for i, ln in enumerate(f, start=1):
                    # відображаємо рядки з номером
                    self.log(f"{i}: {ln.rstrip()}")
            self.log("----- Кінець TF_2 -----")
        except Exception as e:
            messagebox.showerror("Помилка читання TF_2", str(e))
            self.log(f"Помилка читання TF_2: {e}")

if __name__ == "__main__":
    root = tk.Tk()
    app = App(root)
    root.mainloop()
