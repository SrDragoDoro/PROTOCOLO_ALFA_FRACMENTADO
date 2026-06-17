using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración de enemigos")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Configuración de oleadas")]
    [SerializeField] private int enemigosIniciales = 3;
    [SerializeField] private int incrementoPorOleada = 2;
    [SerializeField] private float tiempoEntreOleadas = 5f;

    [Header("Estado actual (solo lectura)")]
    [SerializeField] private int oleadaActual = 0;

    private readonly List<GameObject> enemigosVivos = new List<GameObject>();
    private bool esperandoSiguienteOleada = false;

    private void Start()
    {
        StartCoroutine(CicloDeOleadas());
    }

    private IEnumerator CicloDeOleadas()
    {
        while (true)
        {
            oleadaActual++;
            int cantidadEnemigos = enemigosIniciales + (incrementoPorOleada * (oleadaActual - 1));

            SpawnOleada(cantidadEnemigos);

            // Espera hasta que mueran todos los enemigos de esta oleada
            yield return new WaitUntil(() => enemigosVivos.Count == 0);

            // Espera el tiempo de descanso antes de la siguiente oleada
            esperandoSiguienteOleada = true;
            yield return new WaitForSeconds(tiempoEntreOleadas);
            esperandoSiguienteOleada = false;
        }
    }

    private void SpawnOleada(int cantidad)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("[EnemySpawner] No hay Prefab de enemigo asignado.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] No hay puntos de aparición asignados.");
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            Transform punto = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject nuevoEnemigo = Instantiate(enemyPrefab, punto.position, punto.rotation);

            enemigosVivos.Add(nuevoEnemigo);

            // Se entera automáticamente cuando este enemigo es destruido (muere)
            StartCoroutine(MonitorearEnemigo(nuevoEnemigo));
        }
    }

    private IEnumerator MonitorearEnemigo(GameObject enemigo)
    {
        // Espera mientras el enemigo siga existiendo
        yield return new WaitWhile(() => enemigo != null);

        // Cuando es destruido (null), lo sacamos de la lista de vivos
        enemigosVivos.Remove(enemigo);
    }

    // Útil para mostrar en pantalla cuántos enemigos quedan, o en qué oleada está
    public int EnemigosVivos => enemigosVivos.Count;
    public int OleadaActual => oleadaActual;
    public bool EsperandoSiguienteOleada => esperandoSiguienteOleada;
}