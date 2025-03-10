using UnityEngine;

public static class Utilites
{
    /// <summary>
    /// Retourne une position entre A et B qui se situe à une distance x de A
    /// </summary>
    /// <param name="a">Position du 1er objet</param>
    /// <param name="b">Position du 2e objet</param>
    /// <param name="distanceDeA">Distance à partir de a</param>
    /// <param name="distanceFixe">Est-ce que la position retournée doit toujours être à la même distance ou se rapprocher si b est trop près</param>
    /// <returns>Position sur la droite</returns>
    public static Vector2 getPositionSurDroite(Vector2 a, Vector2 b, float distanceDeA, bool distanceFixe = false)
    {
        if (distanceFixe)
        {
            Vector2 _direction = (b - a).normalized;

            // Calculer la distance à partir de A
            return a + _direction * distanceDeA;
            
        }
        else
        {
            float _distance = Vector2.Distance(a, b);

            return Vector2.Lerp(a, b, distanceDeA / _distance);
        }
    }

    /// <summary>
    /// Renvoie le résultat par composant d'un remappage linéaire d'une valeur x de la plage source [a, b] à la plage de destination [c, d].
    /// </summary>
    /// <param name="valeur">Valeur courante dans l'échelle 1</param>
    /// <param name="echelle1Min">Valeur minimale de l'échelle 1</param>
    /// <param name="echelle1Max">Valeur maximale de l'échelle 1</param>
    /// <param name="echelle2Min">Valeur minimale de l'échelle 2</param>
    /// <param name="echelle2Max">Valeur maximale de l'échelle 2</param>
    /// <returns>Valeur correspondante à l'échelle 2</returns>
    /* Exemple ===========================
    Vitesse d'une voiture. Lorsque la voiture atteint 120km/h, elle expulse des flammes (jusqu'à une vitesse de 180)
    Échelle 1: 120 à 180
    Échelle 2: Quantité de particules: 40 à 100
    Une vitesse de 120 émet 40 particules/s et 180 en émet 100
    La variable "valeur" représente la vitesse courante de la voiture et la fonction retournera la quantité de particules appropriée
    ======================================
    */
    public static float remapper(float valeur, float echelle1Min, float echelle1Max, float echelle2Min, float echelle2Max) 
    {
        // Empêche la valeur d'être en dehors de l'échelle 1
        if (valeur < echelle1Min)
            valeur = echelle1Min;
        else if (valeur > echelle1Max)
            valeur = echelle1Max;

        return (valeur - echelle1Min) / (echelle1Max - echelle1Min) * (echelle2Max - echelle2Min) + echelle2Min;
    }

    /// <summary>
    /// Calcule la rotation d'un objet pour qu'il le regarde (La direction est toujours l'axe x rouge)
    /// </summary>
    /// <param name="objetA">Objet à tourner</param>
    /// <param name="objetB">Cible que l'objetA tente de regarder</param>
    /// <param name="inverse">Permet de faire l'opposer (regarder dans la direction opposée la cible</param>
    public static void rotationVersCible(Transform objetA, Transform objetB, bool inverse = false)
    {
        if (!inverse)
            objetA.right = objetB.position - objetA.position;
        else 
            objetA.right = objetA.position - objetB.position;
    }
}