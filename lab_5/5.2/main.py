import tkinter as tk
from tkinter import ttk, messagebox
import atexit
import sys


# -------------------- 1. Ієрархія Класів (ВИПРАВЛЕНО: Класи йдуть ПЕРШИМИ) --------------------

class Examination:
    """Базовий клас для всіх видів випробувань."""

    def __init__(self, subject="Undefined Subject", max_score=100, is_mandatory=True):
        """Конструктор за замовчуванням, ініціалізації та перезавантаження."""
        self.subject = subject
        self.max_score = max_score
        self.is_mandatory = is_mandatory
        self.date = "N/A"
        self.duration_minutes = 60
        self.id_code = "EX001"

    def __del__(self):
        """Деструктор: виводить повідомлення в консоль."""
        sys.stderr.write("\n[DESTRUCTOR] Лабораторна робота виконана студентом 2 курсу ПІБ студента.\n")

    def process_result(self, student_score):
        """Обробка результату випробування (Базова реалізація)."""
        if student_score > self.max_score: student_score = self.max_score
        percentage = (student_score / self.max_score) * 100
        return f"Базовий результат: {student_score}/{self.max_score} ({percentage:.1f}%)"

    def Show(self):
        """Метод Show(): виводить дані про об'єкт класу в консоль."""
        print(f"\n--- Examination: {self.__class__.__name__} ---")
        print(f"Предмет: {self.subject}, Макс. балів: {self.max_score}")
        return f"Клас: {self.__class__.__name__}, Предмет: {self.subject}, Макс. балів: {self.max_score}"


class Test(Examination):
    """Похідний клас: Тест."""

    def __init__(self, subject="IT Test", max_score=30, is_mandatory=False, question_count=10):
        super().__init__(subject, max_score, is_mandatory)
        self.question_count = question_count
        self.is_online = True
        self.pass_threshold = max_score * 0.6
        self.duration_minutes = 30

    def process_result(self, student_score):
        """Обробка результату тесту (просто Pass/Fail)."""
        if student_score >= self.pass_threshold:
            status = "Здано (PASS)"
        else:
            status = "Не здано (FAIL)"
        return f"Результат Тесту: {student_score}/{self.max_score}. Необхідно {self.pass_threshold:.0f} балів. Статус: {status}"

    def Show(self):
        base_info = super().Show()
        print(f"Кількість питань: {self.question_count}")
        return f"{base_info}, Питань: {self.question_count}, Пороговий бал: {self.pass_threshold}"


class Exam(Examination):
    """Похідний клас: Іспит."""

    def __init__(self, subject="Math Exam", max_score=100, is_mandatory=True, tickets_count=20):
        super().__init__(subject, max_score, is_mandatory)
        self.tickets_count = tickets_count
        self.includes_oral_part = True
        self.commission_size = 3
        self.duration_minutes = 90

    def process_result(self, student_score):
        """Обробка результату іспиту (виведення оцінки)."""
        percentage = (student_score / self.max_score) * 100

        if percentage >= 90:
            grade = "Відмінно (5)"
        elif percentage >= 75:
            grade = "Добре (4)"
        elif percentage >= 60:
            grade = "Задовільно (3)"
        else:
            grade = "Незадовільно (2)"

        return f"Результат Іспиту: {student_score}/{self.max_score}. %: {percentage:.1f}. Оцінка: {grade}"

    def Show(self):
        base_info = super().Show()
        print(f"Кількість білетів: {self.tickets_count}")
        return f"{base_info}, Білетів: {self.tickets_count}, Комісія: {self.commission_size}"


class GraduationExam(Exam):
    """Похідний клас: Випускний іспит (успадковує від Exam)."""

    def __init__(self, subject="State Exam", max_score=100, is_mandatory=True, tickets_count=30,
                 is_thesis_defence=True):
        super().__init__(subject, max_score, is_mandatory, tickets_count)
        self.is_thesis_defence = is_thesis_defence
        self.board_members = 5
        self.certificate_required = True
        self.duration_minutes = 120

    def process_result(self, student_score):
        """Обробка результату випускного іспиту."""
        exam_result = super().process_result(student_score)
        return f"Випускний: {exam_result}. Результат затверджено колегіально ({self.board_members} членів)."

    def Show(self):
        base_info = super().Show()
        print(f"Захист дипломної: {'Так' if self.is_thesis_defence else 'Ні'}")
        return f"{base_info}, Захист: {'Так' if self.is_thesis_defence else 'Ні'}, Члени комісії: {self.board_members}"


# -------------------- 2. GUI (Tkinter) --------------------

class ExaminationApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Лабораторна робота: Ієрархія Класів (Python + GUI)")

        self.current_object = None
        self.create_widgets()

    def create_widgets(self):
        # ------------------- Секція Вводу -------------------
        input_frame = ttk.LabelFrame(self.root, text="Ввід даних та Створення Об'єкта")
        input_frame.pack(padx=10, pady=10, fill="x")

        # Поле 1: Тип випробування
        ttk.Label(input_frame, text="Тип:").grid(row=0, column=0, padx=5, pady=5, sticky="w")
        self.type_combo = ttk.Combobox(input_frame, values=["Test", "Exam", "GraduationExam", "Examination"])
        self.type_combo.set("Test")
        self.type_combo.grid(row=0, column=1, padx=5, pady=5, sticky="ew")

        # Поле 2: Предмет
        ttk.Label(input_frame, text="Предмет:").grid(row=1, column=0, padx=5, pady=5, sticky="w")
        self.subject_entry = ttk.Entry(input_frame)
        self.subject_entry.insert(0, "Історія")
        self.subject_entry.grid(row=1, column=1, padx=5, pady=5, sticky="ew")

        # Поле 3: Макс. балів
        ttk.Label(input_frame, text="Макс. балів:").grid(row=2, column=0, padx=5, pady=5, sticky="w")
        self.max_score_spin = ttk.Spinbox(input_frame, from_=10, to=100)
        self.max_score_spin.set(40)
        self.max_score_spin.grid(row=2, column=1, padx=5, pady=5, sticky="ew")

        # Кнопка Створення
        btn_create = ttk.Button(input_frame, text="1. Створити Об'єкт (Конструктор)", command=self.create_object)
        btn_create.grid(row=3, column=0, columnspan=2, padx=5, pady=10, sticky="ew")

        # ------------------- Секція Демонстрації Методів -------------------

        methods_frame = ttk.LabelFrame(self.root, text="Демонстрація методів")
        methods_frame.pack(padx=10, pady=5, fill="x")

        # Ввід балів для методу process_result
        ttk.Label(methods_frame, text="Набраний бал:").grid(row=0, column=0, padx=5, pady=5, sticky="w")
        self.score_entry = ttk.Spinbox(methods_frame, from_=0, to=100)
        self.score_entry.set(35)
        self.score_entry.grid(row=0, column=1, padx=5, pady=5, sticky="ew")

        # Кнопки методів
        btn_show = ttk.Button(methods_frame, text="2. Викликати Show()", command=self.call_show)
        btn_show.grid(row=1, column=0, padx=5, pady=5, sticky="ew")

        btn_process = ttk.Button(methods_frame, text="3. Викликати process_result() (Поліморфізм)",
                                 command=self.call_process_result)
        btn_process.grid(row=1, column=1, padx=5, pady=5, sticky="ew")

        # ------------------- Секція Результатів -------------------

        output_frame = ttk.LabelFrame(self.root, text="Результати та Вивід (Show() виводить в консоль)")
        output_frame.pack(padx=10, pady=10, fill="both", expand=True)

        self.output_text = tk.Text(output_frame, height=10, width=50)
        self.output_text.pack(padx=5, pady=5, fill="both", expand=True)
        self.output_text.insert(tk.END, "Очікування створення об'єкта...\n")

        # Вихід
        ttk.Button(self.root, text="Вийти (викликає Деструктор)", command=root.destroy).pack(padx=10, pady=10)

    def create_object(self):
        """Створення об'єкта класу на основі введених даних."""
        try:
            subject = self.subject_entry.get()
            max_score = int(self.max_score_spin.get())
            test_type = self.type_combo.get()

            # Створення об'єкта тепер працює, бо класи визначені вище
            if test_type == "Test":
                self.current_object = Test(subject=subject, max_score=max_score, question_count=15)
            elif test_type == "Exam":
                self.current_object = Exam(subject, max_score)
            elif test_type == "GraduationExam":
                self.current_object = GraduationExam(subject, max_score)
            else:
                self.current_object = Examination()

            self.output_text.delete(1.0, tk.END)
            self.output_text.insert(tk.END, f"✅ Успішно створено об'єкт класу: {test_type}\n")
            self.output_text.insert(tk.END,
                                    f"Властивості: Предмет: {self.current_object.subject}, Макс. балів: {self.current_object.max_score}\n")

        except ValueError:
            messagebox.showerror("Помилка вводу", "Переконайтеся, що 'Макс. балів' заповнені коректно.")
        except Exception as e:
            messagebox.showerror("Помилка", f"Сталася помилка при створенні об'єкта: {e}")

    def check_object(self):
        """Перевірка наявності об'єкта."""
        if self.current_object is None:
            messagebox.showwarning("Помилка", "Спершу натисніть 'Створити Об'єкт'.")
            return False
        return True

    def call_show(self):
        """Виклик методу Show()."""
        if self.check_object():
            gui_info = self.current_object.Show()
            self.output_text.insert(tk.END, "\n--- Виклик методу Show() ---\n")
            self.output_text.insert(tk.END,
                                    f"Дані об'єкта виведено в консоль (див. термінал)!\n[GUI Preview]: {gui_info}\n")

    def call_process_result(self):
        """Виклик поліморфного методу process_result()."""
        if self.check_object():
            try:
                student_score = int(self.score_entry.get())

                result_message = self.current_object.process_result(student_score)

                self.output_text.insert(tk.END, "\n--- Виклик методу process_result() ---\n")
                self.output_text.insert(tk.END, result_message + "\n")

            except ValueError:
                messagebox.showerror("Помилка вводу", "Набраний бал має бути цілим числом.")

if __name__ == "__main__":
    root = tk.Tk()
    app = ExaminationApp(root)
    root.mainloop()