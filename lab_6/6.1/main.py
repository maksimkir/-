import tkinter as tk
from tkinter import ttk, messagebox
from abc import ABC, abstractmethod



class Roslyna(ABC):
    """Абстрактний клас Рослина."""

    def __init__(self, nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi):
        self.nazva = nazva
        self.latynska_nazva = latynska_nazva
        self.mistse_zrostannya = mistse_zrostannya
        self.u_chervoniy_knizi = u_chervoniy_knizi

    @abstractmethod
    def kharakterystyka_vydu(self):
        """Повертає унікальну характеристику виду."""
        pass

    @abstractmethod
    def sezonist(self):
        """Повертає сезон цвітіння/плодоношення."""
        pass

    def otrymaty_povnu_informatsiyu(self):
        """Повертає повний рядок інформації про рослину."""
        status = " ТАК (Червона Книга)" if self.u_chervoniy_knizi else " НІ"
        info = f"-- {self.__class__.__name__.upper()} --\n"
        info += f"Назва: {self.nazva} ({self.latynska_nazva})\n"
        info += f"Місце зростання: {self.mistse_zrostannya}\n"
        info += f"Статус: {status}\n"
        info += f"Характеристика: {self.kharakterystyka_vydu()}\n"
        info += f"Сезонність: {self.sezonist()}"
        return info

    def pereviryty_chervonu_knuhu(self):
        """Перевіряє, чи рослина занесена до Червоної книги."""
        return self.u_chervoniy_knizi


class Derevo(Roslyna):
    """Похідний клас Дерево."""

    def __init__(self, nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi, vysota, typ_lystya):
        super().__init__(nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi)
        self.vysota = vysota  # Власне поле
        self.typ_lystya = typ_lystya  # Власне поле ("листяне", "хвойне")

    def kharakterystyka_vydu(self):
        return f"Багаторічна, деревна рослина. Тип листя: {self.typ_lystya}."

    def sezonist(self):
        return "Цвітіння/Плодоношення: весна-літо."

    def skynuty_lystya(self):
        """Моделює процес скидання листя."""
        if self.typ_lystya.lower() == "листяне":
            return f" {self.nazva} ({self.typ_lystya}) скидає листя восени."
        return f" {self.nazva} ({self.typ_lystya}) залишається зеленим цілий рік."

    def otrymaty_povnu_informatsiyu(self):
        base_info = super().otrymaty_povnu_informatsiyu()
        return base_info + f"\nДодатково: Висота {self.vysota} м.\n" + self.skynuty_lystya()


class Kvity(Roslyna):
    """Похідний клас Квіти."""

    def __init__(self, nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi, kolir_pelyustok, tryvalist_zhyttya):
        super().__init__(nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi)
        self.kolir_pelyustok = kolir_pelyustok  # Власне поле
        self.tryvalist_zhyttya = tryvalist_zhyttya  # Власне поле ("однорічна", "багаторічна")

    # Реалізація абстрактних методів
    def kharakterystyka_vydu(self):
        return f"Трав'яниста, квіткова рослина. Тривалість життя: {self.tryvalist_zhyttya}."

    def sezonist(self):
        return "Цвітіння: весна-літо."

    def buket(self):
        """Перевіряє придатність для букету."""
        if self.tryvalist_zhyttya.lower() == "багаторічна":
            return f" {self.nazva} ({self.kolir_pelyustok}) - чудово підходить для букету."
        return f" {self.nazva} - краще залишити рости."

    def otrymaty_povnu_informatsiyu(self):
        base_info = super().otrymaty_povnu_informatsiyu()
        return base_info + f"\nДодатково: Колір пелюсток {self.kolir_pelyustok}.\n" + self.buket()


class PlantApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Лабораторна Робота: Рослинництво (GUI + Ієрархія Класів)")
        self.geometry("900x650")

        self.base_roslyn = self.create_initial_data()

        self.create_widgets()

    def create_initial_data(self):
        """Створення початкової бази (n видів)."""
        return [
            Derevo("Ялиця біла", "Abies alba", "Карпати", True, 40.0, "хвойне"),
            Kvity("Едельвейс", "Leontopodium alpinum", "Карпати", True, "білий", "багаторічна"),
            Derevo("Дуб звичайний", "Quercus robur", "Всюди", False, 35.0, "листяне"),
            Kvity("Шафран Гейфеля", "Crocus heuffelianus", "Закарпаття", True, "фіолетовий", "багаторічна"),
            Kvity("Тюльпан дібровний", "Tulipa quercetorum", "Лісостеп", True, "жовтий", "багаторічна"),
            Derevo("Береза повисла", "Betula pendula", "Полісся", False, 25.0, "листяне"),
        ]

    def create_widgets(self):
        """Створення основних елементів GUI."""
        self.grid_rowconfigure(0, weight=1)
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=0)

        # 1. Рамка для виведення інформації
        info_frame = ttk.LabelFrame(self, text="ℹ️ Інформація та Результати", padding="10")
        info_frame.grid(row=0, column=0, columnspan=2, padx=10, pady=10, sticky="nsew")

        # Текстове поле для виведення
        self.text_output = tk.Text(info_frame, wrap="word", height=20, width=80, font=('Consolas', 10))
        self.text_output.pack(side="left", fill="both", expand=True)

        # Скролбар для текстового поля
        scrollbar = ttk.Scrollbar(info_frame, command=self.text_output.yview)
        scrollbar.pack(side="right", fill="y")
        self.text_output.config(yscrollcommand=scrollbar.set)

        # 2. Рамка для кнопок
        button_frame = ttk.Frame(self, padding="10")
        button_frame.grid(row=1, column=0, padx=10, pady=5, sticky="ew")

        ttk.Button(button_frame, text="Показати всю Базу", command=self.show_all_plants).pack(side="left", padx=5,
                                                                                              pady=5)
        ttk.Button(button_frame, text="Знайти Червону Книгу ", command=self.find_red_book).pack(side="left", padx=5,
                                                                                                 pady=5)

        add_frame = ttk.LabelFrame(self, text="Додати Нову Рослину", padding="10")
        add_frame.grid(row=1, column=1, padx=10, pady=5, sticky="nsew")

        #елементів введення
        self.create_add_form(add_frame)


    def show_all_plants(self):
        """Виводить повну інформацію про всі рослини у базу."""
        self.text_output.delete('1.0', tk.END)
        self.text_output.insert(tk.END, "========== ПОВНА ІНФОРМАЦІЯ ПРО БАЗУ З ДАНІВ ==========\n\n")

        if not self.base_roslyn:
            self.text_output.insert(tk.END, "База даних порожня.\n")
            return

        for idx, plant in enumerate(self.base_roslyn, 1):
            info = plant.otrymaty_povnu_informatsiyu()
            self.text_output.insert(tk.END, f"--- Об'єкт {idx} ---\n{info}\n\n")

    def find_red_book(self):
        """Організовує пошук рослин з Червоної книги."""
        self.text_output.delete('1.0', tk.END)
        self.text_output.insert(tk.END, "========== РЕЗУЛЬТАТИ ПОШУКУ (ЧЕРВОНА КНИГА) ==========\n\n")

        found = False
        for plant in self.base_roslyn:
            if plant.pereviryty_chervonu_knuhu():
                info = plant.otrymaty_povnu_informatsiyu()
                self.text_output.insert(tk.END, f"--- ЗНАЙДЕНО --- \n{info}\n\n")
                found = True

        if not found:
            self.text_output.insert(tk.END, "У базі не знайдено рослин, занесених до Червоної книги України.\n")

    # Gui

    def create_add_form(self, frame):
        """Створює елементи керування для додавання нової рослини."""
        fields = ["Назва:", "Латинська назва:", "Місце зростання:", "У Черв.Книзі? (True/False):"]
        self.entries = {}

        # Загальні поля
        for i, field in enumerate(fields):
            ttk.Label(frame, text=field).grid(row=i, column=0, sticky="w", padx=5, pady=2)
            if "Черв.Книзі" in field:
                # Використовуємо Checkbutton для булевого значення
                self.red_book_var = tk.BooleanVar(value=False)
                entry = ttk.Checkbutton(frame, variable=self.red_book_var)
                entry.grid(row=i, column=1, sticky="ew", padx=5, pady=2)
                self.entries[field] = self.red_book_var
            else:
                entry = ttk.Entry(frame, width=30)
                entry.grid(row=i, column=1, sticky="ew", padx=5, pady=2)
                self.entries[field] = entry

        # Специфічні поля (вибір класу)
        ttk.Label(frame, text="Тип об'єкта:").grid(row=len(fields), column=0, sticky="w", padx=5, pady=2)
        self.class_var = tk.StringVar(value="Дерево")

        class_menu = ttk.Combobox(frame, textvariable=self.class_var, values=["Дерево", "Квіти"], state="readonly")
        class_menu.grid(row=len(fields), column=1, sticky="ew", padx=5, pady=2)
        class_menu.bind("<<ComboboxSelected>>", self.update_specific_fields)

        # Рамка для специфічних полів
        self.specific_frame = ttk.Frame(frame)
        self.specific_frame.grid(row=len(fields) + 1, column=0, columnspan=2, sticky="ew")

        self.specific_entries = {}
        self.update_specific_fields(None)  # Ініціалізація полів для "Дерево"

        # Кнопка додавання
        ttk.Button(frame, text="Додати до Бази", command=self.add_plant_to_base).grid(row=len(fields) + 2, column=0,
                                                                                      columnspan=2, pady=10)

    def update_specific_fields(self, event):
        """Оновлює поля введення залежно від вибраного класу."""
        for widget in self.specific_frame.winfo_children():
            widget.destroy()
        self.specific_entries = {}

        selected_class = self.class_var.get()

        if selected_class == "Дерево":
            fields = ["Висота (м):", "Тип листя (листяне/хвойне):"]
        else:  # Квіти
            fields = ["Колір пелюсток:", "Тривалість життя (однорічна/багаторічна):"]

        for i, field in enumerate(fields):
            ttk.Label(self.specific_frame, text=field).grid(row=i, column=0, sticky="w", padx=5, pady=2)
            entry = ttk.Entry(self.specific_frame, width=30)
            entry.grid(row=i, column=1, sticky="ew", padx=5, pady=2)
            self.specific_entries[field] = entry

    def add_plant_to_base(self):
        """Зчитує дані з форми та додає новий об'єкт до бази."""
        try:
            # Збір загальних даних (trim)
            nazva = self.entries["Назва:"].get().strip()
            latynska_nazva = self.entries["Латинська назва:"].get().strip()
            mistse_zrostannya = self.entries["Місце зростання:"].get().strip()
            u_chervoniy_knizi = self.red_book_var.get()

            # Перевірка на порожні поля
            if not all([nazva, latynska_nazva, mistse_zrostannya]):
                raise ValueError("Заповніть, будь ласка, всі загальні поля.")

            selected_class = self.class_var.get()

            # Збір специфічних даних та створення об'єкта
            if selected_class == "Дерево":
                vysota_str = self.specific_entries["Висота (м):"].get().strip()
                if not vysota_str:
                    raise ValueError("Вкажіть висоту дерева.")
                try:
                    vysota = float(vysota_str)
                except ValueError:
                    raise ValueError("Висота має бути числом (наприклад: 12.5).")
                typ_lystya = self.specific_entries["Тип листя (листяне/хвойне):"].get().strip()
                if not typ_lystya:
                    raise ValueError("Вкажіть тип листя.")
                new_plant = Derevo(nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi, vysota, typ_lystya)

            elif selected_class == "Квіти":
                kolir_pelyustok = self.specific_entries["Колір пелюсток:"].get().strip()
                tryvalist_zhyttya = self.specific_entries["Тривалість життя (однорічна/багаторічна):"].get().strip()
                if not kolir_pelyustok or not tryvalist_zhyttya:
                    raise ValueError("Заповніть, будь ласка, всі специфічні поля для квітки.")
                new_plant = Kvity(nazva, latynska_nazva, mistse_zrostannya, u_chervoniy_knizi, kolir_pelyustok,
                                  tryvalist_zhyttya)

            else:
                raise ValueError("Невірний тип об'єкта.")

            self.base_roslyn.append(new_plant)
            messagebox.showinfo("Успіх", f"Об'єкт '{new_plant.nazva}' успішно додано до бази!")

            # Оновлення відображення
            self.show_all_plants()

        except ValueError as e:
            messagebox.showerror("Помилка вводу", str(e))
        except Exception as e:
            messagebox.showerror("Невідома помилка", f"Сталася непередбачена помилка: {e}")


if __name__ == "__main__":
    app = PlantApp()
    app.mainloop()
