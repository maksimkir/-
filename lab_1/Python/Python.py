import random
 
def generate_random_numbers(R):
  
  x = random.uniform(-R, R)
  y = random.uniform(-R, R)
  return round(x, 2), round(y, 2)

def is_hit(x, y, R):

  if x >= 0 and y >= 0 and x**2 + y**2 <= R**2:
    return True
  if x <= 0 and y <= 0 and x**2 + y**2 <= R**2:
    return True
  return False

def main():
    R = 10
    n_shot =10
    results = []
    
    for i in range(n_shot):
        x, y = generate_random_numbers(R)
        hit = is_hit(x, y, R)
        result = "потрапив у мішень" if hit else "не потрапив у мішень"
        results.append((i, (x, y), result))
        
    print(f"{'№ пострілу':^10}|{'Кординати пострілу':^25}|{'Результат':^25}") 
    print("-"*65)
    for num, cords, res in results:
        print(f"{num:^10}|{str(cords):^25}|{res:^25}")   
        
if __name__ == "__main__":
    main()        
        